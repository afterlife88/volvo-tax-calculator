using System.Net;

namespace Volvo.TaxCalculator.Utils;

public abstract class HttpStatusException : Exception
{
    public abstract HttpStatusCode Status { get; }

    protected HttpStatusException(string message)
        : base(message)
    {
    }
}