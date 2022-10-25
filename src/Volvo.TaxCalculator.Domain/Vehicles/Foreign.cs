namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Foreign : IVehicle
{
    public string GetVehicleType() => nameof(Foreign);
}