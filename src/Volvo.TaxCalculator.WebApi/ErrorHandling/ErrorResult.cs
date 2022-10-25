namespace Volvo.TaxCalculator.WebApi.ErrorHandling;

public sealed class ErrorResult
{
    public string RequestId { get; }

    public IEnumerable<string> Errors { get; }

    public ErrorResult(IEnumerable<string> errors, string requestId)
    {
        Errors = errors;
        RequestId = requestId;
    }

    public override string ToString() => string.Join(Environment.NewLine, Errors);
}