using System;
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
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository, 
            ISimulatedBankService simulatedBankService, 
            ITransactionService transactionService, 
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _simulatedBankService = simulatedBankService;
            _transactionService = transactionService;
            _mapper = mapper;
        }
        
        public async Task<ProcessPaymentResult> ProcessPayment(PaymentDetails paymentDetails)
        {
            try
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

                var transactionSaved = await _transactionService.SaveTransactionDetails(transactionDetails);
            
                return transactionSaved 
                    ? new ProcessPaymentResult(transactionDetails.Success, transactionDetails.TransactionID)
                    : ProcessPaymentResult.Failed;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ProcessPaymentResult.Failed;
            }
        }
    }
}