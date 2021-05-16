namespace PaymentGateway.Contracts
{
    public class SimulatedBankResponse
    {
        public SimulatedBankResponse(long transactionId, TransactionStatus status)
        {
            TransactionID = transactionId;
            Status = status;
        }
        
        public long TransactionID { get; }
        public TransactionStatus Status { get; }
    }

    public enum TransactionStatus
    {
        Success,
        Fail
    }
}