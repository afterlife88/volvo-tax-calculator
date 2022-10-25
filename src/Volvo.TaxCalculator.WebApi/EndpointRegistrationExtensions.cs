using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Volvo.TaxCalculator.WebApi;

public static class EndpointRegistrationExtensions
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapPost("calculate-fee", async (
                [FromServices] ITaxCalculatorService service,
                IValidator<CalculateTaxRequest> validator,
                [FromBody] CalculateTaxRequest request) =>
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                return await service.ExecuteAsync(request);
            })
            .Produces<int>()
            .AllowAnonymous();
    }
}