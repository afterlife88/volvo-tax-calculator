namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Tractor : IVehicle
{
    public string GetVehicleType() => nameof(Tractor);
}