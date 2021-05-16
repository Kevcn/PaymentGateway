using System.Threading.Tasks;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Repository
{
    public interface IPaymentRepository
    {
        Task<int> SavePaymentDetails(PaymentDetailsDTO paymentDetails);
    }
}