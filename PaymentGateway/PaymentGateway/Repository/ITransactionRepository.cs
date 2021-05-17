using System.Threading.Tasks;
using PaymentGateway.Domain;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Repository
{
    public interface ITransactionRepository
    {
        Task<bool> SaveTransactionDetails(TransactionDetailsDTO transactionDetails);
        
        Task<TransactionHistory> GetTransactionHistoryById(long transactionID);

    }
}