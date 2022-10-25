using System.Net;
using Volvo.TaxCalculator.Utils;

namespace Volvo.TaxCalculator.Domain;

public class CongestionBusinessException : HttpStatusException
{
    public override HttpStatusCode Status => HttpStatusCode.UnprocessableEntity;

    public CongestionBusinessException(string message) : base(message)
    {
    }
}