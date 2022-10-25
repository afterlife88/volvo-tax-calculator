using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Volvo.TaxCalculator.WebApi.IntegrationTests;

public sealed class CalculateFeeEndpointTests
{
    [Theory]
    [InlineData("Gothenburg", HttpStatusCode.OK)]
    [InlineData("Copenhagen", HttpStatusCode.UnprocessableEntity)]
    [InlineData("", HttpStatusCode.BadRequest)]
    [InlineData(default, HttpStatusCode.BadRequest)]
    public async Task ItCalculatesFeeForValidCity(string cityName, HttpStatusCode expectedStatusCode)
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateClient();
        var result = await httpClient.PostAsJsonAsync("/calculate-fee",
            new CalculateTaxRequest
            (cityName,
                VehicleType.Car,
                new[] { DateTime.Now }));

        Assert.Equal(expectedStatusCode, result.StatusCode);
    }
}