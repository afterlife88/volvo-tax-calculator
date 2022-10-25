using FluentValidation;

namespace Volvo.TaxCalculator.WebApi;

public sealed record CalculateTaxRequest(string City, VehicleType Vehicle, DateTime[] PassDates)
{
    public sealed class Validator : AbstractValidator<CalculateTaxRequest>
    {
        public Validator()
        {
            RuleFor(vsa => vsa.City).NotEmpty();
            RuleFor(vsa => vsa.PassDates).NotEmpty();
            RuleFor(vsa => vsa.Vehicle).IsInEnum();
        }
    }
}

public enum VehicleType
{
    Car = 0,
    Foreign = 1,
    Motorcycle = 2,
    Tractor = 3,
    Military = 4,
    Diplomat = 5,
    Emergency = 6
}