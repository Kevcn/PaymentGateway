namespace PaymentGateway.Contracts.V1.Responses
{
    public class FailedResponse
    {
        public FailedResponse(string status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }
        
        public string Status { get; }
        public string ErrorMessage { get; }
    }
}