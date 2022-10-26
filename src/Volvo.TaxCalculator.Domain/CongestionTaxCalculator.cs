using System.Collections.Immutable;
using System.Text.Json;
using Volvo.TaxCalculator.Domain.Vehicles;
using Volvo.TaxCalculator.Utils;

namespace Volvo.TaxCalculator.Domain;

public sealed class CongestionTaxCalculator : ICongestionTaxCalculator
{
    private readonly IReadOnlyDictionary<string, Task<TaxRules>> _cityFeeRules =
        new Dictionary<string, Task<TaxRules>>
        {
            ["GOTHENBURG"] = GetTaxRulesForCity("gothenburg-rules.json"),

            // Extend for more cities by adding another json rule file in CitiesFeeRules folder
            // ["Copenhagen"] = GetTolCityFee("copenhagen-rules.json"),
        };

    public async Task<int> GetTax(string city, IVehicle vehicle, IEnumerable<DateTime> dates)
    {
        _cityFeeRules.TryGetValue(city.ToUpper(), out var rulesTaskOpt);
        var rulesTask = rulesTaskOpt ??
                        throw new CongestionBusinessException(
                            $"Rules for city {city} is not supported.");
        var cityTaxRules = await rulesTask;

        var sortedDates = dates.OrderBy(d => d).ToImmutableArray();
        var startDate = sortedDates.First();

        var initialFee = GetTollFee(startDate, vehicle, cityTaxRules);
        var totalFee = 0;
        foreach (var date in sortedDates)
        {
            var currentPassFee = GetTollFee(date, vehicle, cityTaxRules);

            // Fix
            // long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            // long minutes = diffInMillies / 1000 / 60;
            var minutesFromInitialPass = date.Subtract(startDate).TotalMinutes;

            if (minutesFromInitialPass <= 60)
            {
                if (totalFee > 0)
                    totalFee -= initialFee;
                if (currentPassFee >= initialFee)
                    initialFee = currentPassFee;
                totalFee += initialFee;
            }
            else
            {
                // Fix by moving start date pointer
                startDate = date;
                totalFee += currentPassFee;
            }
        }

        if (totalFee > 60)
            totalFee = 60;
        return totalFee;
    }

    private static async Task<TaxRules> GetTaxRulesForCity(string cityRuleFile)
    {
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CitiesFeeRules", cityRuleFile);
        var rulesString = await File.ReadAllTextAsync(fullPath);
        var options = new JsonSerializerOptions();
        options.Converters.Add(new TimeOnlyConverter());
        var rules = JsonSerializer.Deserialize<TaxRules>(rulesString, options)!;
        return rules;
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        var year = date.Year;
        var month = date.Month;
        var day = date.Day;

        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return true;

        if (year != 2013)
            return false;
        return month == 1 && day == 1 ||
               month == 3 && day is 28 or 29 ||
               month == 4 && day is 1 or 30 ||
               month == 5 && day is 1 or 8 or 9 ||
               month == 6 && day is 5 or 6 or 21 ||
               month == 7 ||
               month == 11 && day == 1 ||
               month == 12 && day is 24 or 25 or 26 or 31;
    }

    private static bool IsTollFreeVehicle(IVehicle vehicle)
    {
        var vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    private int GetTollFee(DateTime date, IVehicle vehicle, TaxRules cityTaxRules)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
            return 0;

        var timeOnlyPass = TimeOnly.FromDateTime(date);

        return cityTaxRules.HourRules
            .Where(x => timeOnlyPass >= x.From && timeOnlyPass <= x.To)
            .Select(x => x.Tax).FirstOrDefault();
    }
}