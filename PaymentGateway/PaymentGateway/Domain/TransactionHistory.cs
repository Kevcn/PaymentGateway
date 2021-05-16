namespace PaymentGateway.Domain
{
    public class TransactionHistory
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool Success { get; set; }
    }
}