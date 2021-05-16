using System.Threading.Tasks;
using PaymentGateway.Domain;

namespace PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<ProcessPaymentResult> ProcessPayment(PaymentDetails paymentDetails);
    }
}