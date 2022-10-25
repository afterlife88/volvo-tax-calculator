namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Diplomat : IVehicle
{
    public string GetVehicleType() => nameof(Diplomat);
}