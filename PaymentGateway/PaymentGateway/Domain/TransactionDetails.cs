using System;

namespace PaymentGateway.Domain
{
    public class TransactionDetails
    {
        public TransactionDetails(long transactionId, bool success, int paymentDetailsId)
        {
            TransactionID = transactionId;
            Success = success;
            PaymentDetailsID = paymentDetailsId;
            CreatedDate = DateTime.UtcNow;
        }
        
        public long TransactionID { get; }
        public bool Success { get; }
        public int PaymentDetailsID { get; }
        public DateTime CreatedDate { get; }
    }
}