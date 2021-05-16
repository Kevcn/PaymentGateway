using System.Threading.Tasks;
using PaymentGateway.Domain;

namespace PaymentGateway.Services
{
    public interface ITransactionService
    {
        Task<bool> SaveTransactionDetails(TransactionDetails transactionDetails);

        Task<TransactionHistory> GetTransactionHistory(long transactionID);
    }
}