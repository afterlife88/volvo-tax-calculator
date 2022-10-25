namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Military : IVehicle
{
    public string GetVehicleType() => nameof(Military);
}