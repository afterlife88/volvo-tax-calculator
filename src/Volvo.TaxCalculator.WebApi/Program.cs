using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using Volvo.TaxCalculator.Domain;
using Volvo.TaxCalculator.WebApi;
using Volvo.TaxCalculator.WebApi.ErrorHandling;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "Volvo Tax Calculator");
    x.RoutePrefix = string.Empty;
});

app.RegisterEndpoints();
app.UseHttpsRedirection();
app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddMvcCore()
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    
    services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    services.AddEndpointsApiExplorer();
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    services.AddTransient<ITaxCalculatorService, TaxCalculatorService>();
    services.AddTransient<ICongestionTaxCalculator, CongestionTaxCalculator>();

    services.AddSwaggerGen(options =>
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Volvo Tax Calculator", Version = "v1" }));
}