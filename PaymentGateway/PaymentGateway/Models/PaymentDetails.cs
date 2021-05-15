namespace PaymentGateway.Models
{
    public class PaymentDetails
    {
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryDate { get; set; }
        public string CardHolderName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CVV { get; set; }
    }
}