using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using PaymentGateway.SimulatedBank;

namespace PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISimulatedBankService _simulatedBankService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository, 
            ISimulatedBankService simulatedBankService, 
            ITransactionRepository transactionRepository, 
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _simulatedBankService = simulatedBankService;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        
        public async Task<ProcessPaymentResult> ProcessPayment(PaymentDetails paymentDetails)
        {
            var paymentDetailsID = await _paymentRepository.SavePaymentDetails(_mapper.Map<PaymentDetailsDTO>(paymentDetails));

            var bankResponse = await _simulatedBankService.GetBankResponse(paymentDetails);

            if (bankResponse.Status == TransactionStatus.Fail)
            {
                return ProcessPaymentResult.Failed;
            }
            
            var transactionDetails = new TransactionDetails(
                bankResponse.TransactionID,
                bankResponse.Status == TransactionStatus.Success,
                paymentDetailsID
            );

            var transactionSaved = await _transactionRepository.SaveTransactionDetails(_mapper.Map<TransactionDetailsDTO>(transactionDetails));
            
            return transactionSaved 
                ? new ProcessPaymentResult(transactionDetails.Success, transactionDetails.TransactionID)
                : ProcessPaymentResult.Failed;
        }
    }
}