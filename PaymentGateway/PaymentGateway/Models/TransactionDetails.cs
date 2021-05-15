namespace PaymentGateway.Models
{
    public class TransactionDetails
    {
        public long ID { get; set; }
        public TransactionStatus transactionStatus { get; set; }
    }

    public enum TransactionStatus
    {
        Succeed,
        Fail
    }
}