namespace PaymentGateway.Domain
{
    public class ProcessPaymentResult
    {
        public static readonly ProcessPaymentResult Failed = new ProcessPaymentResult(false, 0);
        
        public ProcessPaymentResult(bool success, long transactionId)
        {
            Success = success;
            TransactionID = transactionId;
        }
        public bool Success { get; }
        public long TransactionID { get; }
    }
}