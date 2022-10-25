namespace Volvo.TaxCalculator.Domain.Vehicles;

public sealed record Tractor : IVehicle
{
    public string GetVehicleType()
    {
        return "Tractor";
    }
}