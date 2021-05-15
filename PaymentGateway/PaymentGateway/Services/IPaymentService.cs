using System.Threading.Tasks;
using PaymentGateway.Models;

namespace PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(PaymentDetails paymentDetails);
    }
}