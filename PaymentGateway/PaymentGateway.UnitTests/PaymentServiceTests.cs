using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PaymentGateway.Configurations.Mappings;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using PaymentGateway.Services;
using PaymentGateway.SimulatedBank;
using Serilog;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class PaymentServiceTests
    {
        private readonly PaymentService _paymentService;
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly Mock<ISimulatedBankService> _mockSimulatedBankService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<ILogger> _mockLogger;

        public PaymentServiceTests()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockSimulatedBankService = new Mock<ISimulatedBankService>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mockLogger = new Mock<ILogger>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainToDTO>());
            var mapper = config.CreateMapper();

            _paymentService = new PaymentService(
                _mockPaymentRepository.Object,
                _mockSimulatedBankService.Object,
                _mockTransactionService.Object,
                mapper,
                _mockLogger.Object);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnSuccessResult_WhenBankReturnsSuccessResponse()
        {
            var expectedTransactionID = 9999;
            
            var paymentDetails = new PaymentDetails
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };
            
            _mockPaymentRepository.Setup(x => x.SavePaymentDetails(It.IsAny<PaymentDetailsDTO>())).ReturnsAsync(123);
            _mockSimulatedBankService.Setup(x => x.GetBankResponse(paymentDetails))
                .ReturnsAsync(new SimulatedBankResponse(expectedTransactionID, TransactionStatus.Success));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(true);

            var actual = await _paymentService.ProcessPayment(paymentDetails);

            Assert.True(actual.Success);
            Assert.Equal(expectedTransactionID, actual.TransactionID);
        }

        [Fact]
        public async Task ProcessPayment_ShouldReturnFailedResult_WhenBankReturnsFailedResponse()
        {
            var expectedTransactionID = 9998;

            var paymentDetails = new PaymentDetails
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };
            
            _mockPaymentRepository.Setup(x => x.SavePaymentDetails(It.IsAny<PaymentDetailsDTO>())).ReturnsAsync(123);
            _mockSimulatedBankService.Setup(x => x.GetBankResponse(paymentDetails))
                .ReturnsAsync(new SimulatedBankResponse(expectedTransactionID, TransactionStatus.Fail));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(true);

            var actual = await _paymentService.ProcessPayment(paymentDetails);
            
            Assert.False(actual.Success);
            Assert.Equal(expectedTransactionID, actual.TransactionID);
        }
    }
}