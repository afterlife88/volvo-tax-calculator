using Microsoft.AspNetCore.Mvc;

namespace Volvo.TaxCalculator.WebApi;

public static class EndpointRegistrationExtensions
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        app.MapPost("calculate-fee", (
                [FromServices] ITaxCalculatorService service,
                [FromBody] CalculateTaxModel model) => service.ExecuteAsync(model))
            .AllowAnonymous();
    }
}