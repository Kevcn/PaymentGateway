using System.Threading.Tasks;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;

namespace PaymentGateway.SimulatedBank
{
    public class SimulatedBankService : ISimulatedBankService
    {
        public async Task<SimulatedBankResponse> GetBankResponse(PaymentDetails paymentDetails)
        {
            // Simulating bank response
            
            var response = new SimulatedBankResponse((long)paymentDetails.Amount + 1, TransactionStatus.Success);
            
            return await Task.FromResult(response);
        }
    }
}