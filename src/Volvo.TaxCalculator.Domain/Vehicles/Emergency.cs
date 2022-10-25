namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Emergency : IVehicle
{
    public string GetVehicleType() => nameof(Emergency);
}