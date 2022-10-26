using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volvo.TaxCalculator.Domain.Vehicles;
using Xunit;

namespace Volvo.TaxCalculator.Domain.Tests;

public sealed class CongestionTaxCalculatorTests
{
    private const string CityToTest = "Gothenburg";

    [Theory]

    // Validate basic rules
    [InlineData(8, "2013-02-08 06:20", "2013-02-08 06:27")]
    [InlineData(13, "2013-02-08 06:30", "2013-02-08 06:59")]
    [InlineData(18, "2013-02-08 07:00", "2013-02-08 07:59")]
    [InlineData(13, "2013-02-08 08:00", "2013-02-08 08:29")]
    [InlineData(8, "2013-02-08 08:30", "2013-02-08 09:20")]
    [InlineData(13, "2013-02-08 15:00", "2013-02-08 15:29")]
    [InlineData(18, "2013-02-08 15:30", "2013-02-08 16:15")]
    [InlineData(0, "2013-02-08 18:30", "2013-02-08 19:30")]

    // More then 1h rule
    [InlineData(16, "2013-02-08 08:30", "2013-02-08 14:59")]
    [InlineData(36, "2013-02-08 15:30", "2013-02-08 16:59")]

    // Complex trips (edge cases) from colleague desk
    [InlineData(0, "2013-01-14 21:00", "2013-01-15 21:00")]
    [InlineData(21, "2013-02-07 06:23", "2013-02-07 15:27")]
    [InlineData(16, "2013-02-08 06:20", "2013-02-08 14:35")]

    // 15: 29 - 13 SEK, but was during hour in 16:01 so highest price is 18 SEK
    // 16: 48 - 18
    // 17:49 - 13
    [InlineData(49, "2013-02-08 15:29", "2013-02-08 15:47",
        "2013-02-08 16:01", "2013-02-08 16:48", "2013-02-08 17:49", "2013-02-08 18:29", "2013-02-08 18:35")]
    [InlineData(8, "2013-03-26 14:25", "2013-03-28 14:07")]
    public async Task ItGetsTaxAmountCorrectlyByTheRules(int expected, params string[] passes)
    {
        var passesDateTimes = passes
            .Select(str =>
                DateTime.ParseExact(str, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));

        var vehicle = new Car();
        var taxCalculator = new CongestionTaxCalculator();

        var tax = await taxCalculator.GetTax(CityToTest, vehicle, passesDateTimes);

        Assert.Equal(expected, tax);
    }

    [Theory]
    [InlineData(typeof(Motorcycle))]
    [InlineData(typeof(Tractor))]
    [InlineData(typeof(Emergency))]
    [InlineData(typeof(Diplomat))]
    [InlineData(typeof(Foreign))]
    [InlineData(typeof(Military))]
    public async Task ItReturnsZeroForExemptVehicles(Type vehicle)
    {
        var taxCalculator = new CongestionTaxCalculator();
        var tax = await taxCalculator.GetTax(CityToTest, (IVehicle)Activator.CreateInstance(vehicle)!,
            new List<DateTime>() { DateTime.Now });
        Assert.Equal(0, tax);
    }

    [Fact]
    public async Task ItThrowsCongestionBusinessExceptionWhenRequestedForCityWithoutRules()
    {
        var taxCalculator = new CongestionTaxCalculator();
        await
            Assert.ThrowsAsync<CongestionBusinessException>(()
                => taxCalculator.GetTax("TestCity", new Car(), new List<DateTime>() { DateTime.Now }));
    }
}