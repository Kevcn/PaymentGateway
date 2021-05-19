namespace PaymentGateway.Domain
{
    public class ProcessPaymentResult
    {
        public ProcessPaymentResult(bool success, long transactionId)
        {
            Success = success;
            TransactionID = transactionId;
        }
        public bool Success { get; }
        public long TransactionID { get; }
    }
}