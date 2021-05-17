using System;

namespace PaymentGateway.Contracts.V1.Responses
{
    public class TransactionHistoryResponse
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public bool Success { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}