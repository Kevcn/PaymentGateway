using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Repository
{
    public interface IPaymentRepository
    {
        Task<int> SavePaymentDetails(PaymentDetails paymentDetails);
    }
}