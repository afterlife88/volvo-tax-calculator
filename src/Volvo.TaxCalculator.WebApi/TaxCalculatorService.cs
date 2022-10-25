using FluentValidation;
using Volvo.TaxCalculator.Domain;
using Volvo.TaxCalculator.Domain.Vehicles;

namespace Volvo.TaxCalculator.WebApi;

public interface ITaxCalculatorService
{
    Task<int> ExecuteAsync(CalculateTaxModel model);
}

public class TaxCalculatorService : ITaxCalculatorService
{
    // private readonly IValidator<CalculateTaxModel> _validator;
    private readonly ICongestionTaxCalculator _congestionTaxCalculator;

    public TaxCalculatorService(ICongestionTaxCalculator congestionTaxCalculator)
    {
        // _validator = validator;
        _congestionTaxCalculator = congestionTaxCalculator;
    }

    public async Task<int> ExecuteAsync(CalculateTaxModel model)
    {
        // var res = await _validator.ValidateAsync(model);
        // if (!res.IsValid)
        //     throw new ValidationException(res.Errors);


        IVehicle vehicle = model.Vehicle switch
        {
            VehicleType.Car => new Car(),
            VehicleType.Foreign => new Foreign(),
            VehicleType.Motorcycle => new Motorcycle(),
            VehicleType.Tractor => new Tractor(),
            VehicleType.Emergency => new Emergency(),
            VehicleType.Diplomat => new Diplomat(),
            VehicleType.Military => new Military(),
            _ => throw new ArgumentOutOfRangeException(nameof(model.Vehicle), model.Vehicle, "Passed type of vehicle is not supported")
        };

        return await _congestionTaxCalculator.GetTax(model.City, vehicle, model.PassDates);
    }
}