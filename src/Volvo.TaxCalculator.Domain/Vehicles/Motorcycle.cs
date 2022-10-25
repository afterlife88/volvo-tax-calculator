namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Motorcycle : IVehicle
{
    public string GetVehicleType() => nameof(Motorcycle);
}