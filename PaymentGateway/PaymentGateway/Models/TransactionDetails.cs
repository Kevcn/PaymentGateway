using System;

namespace PaymentGateway.Models
{
    public class TransactionDetails
    {
        public long TransactionID { get; set; }
        public bool Success { get; set; }
        public int paymentDetailsID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}