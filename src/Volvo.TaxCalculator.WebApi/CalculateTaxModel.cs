namespace Volvo.TaxCalculator.WebApi;

public record CalculateTaxModel
{
    public string City { get; init; }
    public VehicleType Vehicle { get; init; }
    public DateTime[] PassDates { get; set; }
}

public enum VehicleType
{
    Car,
    Foreign,
    Motorcycle,
    Tractor,
    Military,
    Diplomat,
    Emergency
}