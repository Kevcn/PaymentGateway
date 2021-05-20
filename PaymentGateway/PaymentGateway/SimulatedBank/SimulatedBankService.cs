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
            var transactionId = random.Next();
            
            var response = new SimulatedBankResponse(transactionId, 
                transactionId % 2 == 0 ? TransactionStatus.Success : TransactionStatus.Fail);
            
            return await Task.FromResult(response);
        }
    }
}