namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Car : IVehicle
{
    public string GetVehicleType() => nameof(Car);
}