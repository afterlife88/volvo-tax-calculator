namespace Volvo.TaxCalculator.WebApi;

public interface ITaxCalculatorService
{
    Task<int> ExecuteAsync(CalculateTaxRequest model);
}