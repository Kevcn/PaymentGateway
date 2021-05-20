using System;

namespace PaymentGateway.Services
{
    public interface IUriService
    {
        Uri GetTransactionUri(long transactionId);
    }
}