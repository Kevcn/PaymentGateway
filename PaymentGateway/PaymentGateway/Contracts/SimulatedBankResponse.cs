namespace PaymentGateway.Contracts
{
    public class SimulatedBankResponse
    {
        public long TransactionID { get; set; }
        public TransactionStatus Status { get; set; }
    }

    public enum TransactionStatus
    {
        Succeed,
        Fail
    }
}