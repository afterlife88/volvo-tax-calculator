using Volvo.TaxCalculator.Domain.Vehicles;

namespace Volvo.TaxCalculator.Domain;

public interface ICongestionTaxCalculator
{
    Task<int> GetTax(string city, IVehicle vehicle, IEnumerable<DateTime> dates);
}