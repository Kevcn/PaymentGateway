using System;

namespace PaymentGateway.Repository.DTO
{
    public class TransactionDetailsDTO
    {
        public long TransactionID { get; set; }
        public bool Success { get; set; }
        public int PaymentDetailsID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}