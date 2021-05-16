using System.Collections.Generic;

namespace PaymentGateway.Contracts.V1.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            
        }
        public ErrorResponse(ErrorModel errorModel)
        {
            Errors.Add(errorModel);
        }

        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}