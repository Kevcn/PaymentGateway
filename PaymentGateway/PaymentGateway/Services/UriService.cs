using System;
using PaymentGateway.Contracts;

namespace PaymentGateway.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetTransactionUri(long transactionId)
        {
            return new Uri(_baseUri + ApiRoutes.Transaction.Get.Replace("{transactionId}", transactionId.ToString()));
        }
    }
}