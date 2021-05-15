using System.Threading.Tasks;
using PaymentGateway.Contracts;
using PaymentGateway.Models;

namespace PaymentGateway.SimulatedBank
{
    public class SimulatedBankService : ISimulatedBankService
    {
        public async Task<SimulatedBankResponse> GetBankResponse(PaymentDetails paymentDetails)
        {
            throw new System.NotImplementedException();
        }
    }
}