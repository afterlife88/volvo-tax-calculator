using System.Net;
using System.Runtime.Serialization;

namespace Volvo.TaxCalculator.Utils;

public abstract class HttpStatusException : Exception
{
    public abstract HttpStatusCode Status { get; }

    protected HttpStatusException(string message)
        : base(message)
    {
    }

    protected HttpStatusException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected HttpStatusException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}