namespace PaymentGateway.Contracts.V1.Responses
{
    public class FailedResponse
    {
        public FailedResponse(string status, long transactionId, string errorMessage)
        {
            Status = status;
            TransactionID = transactionId;
            ErrorMessage = errorMessage;
        }
        
        public string Status { get; }
        public long TransactionID { get; }
        public string ErrorMessage { get; }
    }
}