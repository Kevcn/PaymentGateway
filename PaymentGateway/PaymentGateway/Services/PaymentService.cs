using System.Threading.Tasks;
using PaymentGateway.Models;
using PaymentGateway.Repository;
using PaymentGateway.SimulatedBank;

namespace PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISimulatedBankService _simulatedBankService;

        public PaymentService(IPaymentRepository paymentRepository, ISimulatedBankService simulatedBankService)
        {
            _paymentRepository = paymentRepository;
            _simulatedBankService = simulatedBankService;
        }
        
        public async Task<bool> ProcessPayment(PaymentDetails paymentDetails)
        {
            var paymentDetailsID = await _paymentRepository.SavePaymentDetails(paymentDetails);

            var paymentProcessResult = await _simulatedBankService.GetBankResponse(paymentDetails);

            return true;
        }
    }
}