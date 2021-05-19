using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using PaymentGateway.SimulatedBank;
using Serilog;

namespace PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISimulatedBankService _simulatedBankService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentService(
            IPaymentRepository paymentRepository, 
            ISimulatedBankService simulatedBankService, 
            ITransactionService transactionService, 
            IMapper mapper, ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _simulatedBankService = simulatedBankService;
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<ProcessPaymentResult> ProcessPayment(PaymentDetails paymentDetails)
        {
            var paymentDetailsID = await _paymentRepository.SavePaymentDetails(_mapper.Map<PaymentDetailsDTO>(paymentDetails));

            var bankResponse = await _simulatedBankService.GetBankResponse(paymentDetails);
        
            _logger.Information($"Received bank response for payment {paymentDetailsID}: Status - {bankResponse.Status}, TransactionID - {bankResponse.TransactionID}");
            
            var transactionDetails = new TransactionDetails(
                bankResponse.TransactionID,
                bankResponse.Status == TransactionStatus.Success,
                paymentDetailsID
            );

            await _transactionService.SaveTransactionDetails(transactionDetails);

            return new ProcessPaymentResult(transactionDetails.Success, transactionDetails.TransactionID);
        }
    }
}