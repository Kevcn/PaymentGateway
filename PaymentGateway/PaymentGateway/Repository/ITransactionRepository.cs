using System.Threading.Tasks;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Repository
{
    public interface ITransactionRepository
    {
        Task<bool> SaveTransactionDetails(TransactionDetailsDTO transactionDetails);
    }
}