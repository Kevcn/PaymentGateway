using System;
using System.Threading.Tasks;
using AutoMapper;
using MySql.Data.MySqlClient;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using PaymentGateway.SimulatedBank;
using Polly;
using Polly.Retry;
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
        private readonly AsyncRetryPolicy _retryPolicy;

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
            _retryPolicy = Policy.Handle<MySqlException>().WaitAndRetryAsync(
                3, retryAttempt => TimeSpan.FromMilliseconds(retryAttempt * 100),
                (result, duration, retryCount, context) => { LogRetry(retryCount); });
        }
        
        public async Task<ProcessPaymentResult> ProcessPayment(PaymentDetails paymentDetails)
        {
            // Wraps the call to repo with polly policy defined above
            return await ExecuteWithRetry(async () =>
            {
                var paymentDetailsID =
                    await _paymentRepository.SavePaymentDetails(_mapper.Map<PaymentDetailsDTO>(paymentDetails));

                var bankResponse = await _simulatedBankService.GetBankResponse(paymentDetails);

                _logger
                    // ForContext - The propertyName will be displayed as a field in ElasticSearch. This is more concise and clean
                    .ForContext("PaymentDetailsID", paymentDetailsID)
                    // Example of destructing object built into Serilog
                    .Information("Received bank response {@BankResponse}", bankResponse);
                
                var transactionDetails = new TransactionDetails(
                    bankResponse.TransactionID,
                    bankResponse.Status == TransactionStatus.Success,
                    paymentDetailsID
                );

                await _transactionService.SaveTransactionDetails(transactionDetails);

                return new ProcessPaymentResult(transactionDetails.Success, transactionDetails.TransactionID);
            });
        }

        private async Task<ProcessPaymentResult> ExecuteWithRetry(Func<Task<ProcessPaymentResult>> action)
        {
            return await _retryPolicy.ExecuteAsync(action);
        }

        private void LogRetry(int retryCount)
        {
            _logger.Warning("Attempting to process payment result, Attempt: {RetryCount}", retryCount);
        }
    }
}