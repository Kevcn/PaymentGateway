using System.Threading.Tasks;
using PaymentGateway.Contracts;
using PaymentGateway.Models;

namespace PaymentGateway.SimulatedBank
{
    public interface ISimulatedBankService
    {
        Task<SimulatedBankResponse> GetBankResponse(PaymentDetails paymentDetails);
    }
}