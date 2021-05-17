using System;
using System.Threading.Tasks;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;

namespace PaymentGateway.SimulatedBank
{
    public class SimulatedBankService : ISimulatedBankService
    {
        public async Task<SimulatedBankResponse> GetBankResponse(PaymentDetails paymentDetails)
        {
            // Fake bank response

            Random random = new Random();
            
            var response = new SimulatedBankResponse(random.Next(), TransactionStatus.Success);
            
            return await Task.FromResult(response);
        }
    }
}