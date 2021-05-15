namespace PaymentGateway.Contracts.V1.Responses
{
    public class SuccessResponse
    {
        public string Status { get; set; }
        public long TransactionID { get; set; }
    }
}