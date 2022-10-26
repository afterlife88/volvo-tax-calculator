using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Volvo.TaxCalculator.WebApi.IntegrationTests;

public sealed class CalculateFeeEndpointTests
{
    private const string ValidCity = "Gothenburg";
    private const string EndpointPath = "/calculate-fee";

    [Fact]
    public async Task ItCalculatesFeeByTheRules()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateClient();
        var result = await httpClient.PostAsJsonAsync(
            EndpointPath,
            new CalculateTaxRequest(
                ValidCity,
                VehicleType.Car,
                new DateTime[]
                {
                    new(2013, 2, 8, 6, 20, 0),
                    new(2013, 2, 8, 6, 27, 0),
                }));

        Assert.Equal((HttpStatusCode)200, result.StatusCode);
        Assert.Equal("8", result.Content.ReadAsStringAsync().Result);
    }

    [Theory]
    [InlineData("Gothenburg", HttpStatusCode.OK)]
    [InlineData("Copenhagen", HttpStatusCode.UnprocessableEntity)]
    [InlineData("", HttpStatusCode.BadRequest)]
    [InlineData(default, HttpStatusCode.BadRequest)]
    public async Task ItCalculatesFeeForValidCity(string cityName, HttpStatusCode expectedStatusCode)
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateClient();
        var result = await httpClient.PostAsJsonAsync(
            EndpointPath,
            new CalculateTaxRequest(
                cityName,
                VehicleType.Car,
                new[] { DateTime.Now }));

        Assert.Equal(expectedStatusCode, result.StatusCode);
    }
}