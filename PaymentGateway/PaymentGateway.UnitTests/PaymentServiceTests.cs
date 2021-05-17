using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using PaymentGateway.Services;
using PaymentGateway.SimulatedBank;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class PaymentServiceTests
    {
        private readonly PaymentService _paymentService;
        private readonly Mock<IPaymentRepository> _mockPaymentRepository;
        private readonly Mock<ISimulatedBankService> _mockSimulatedBankService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<IMapper> _mockMapper;
        
        public PaymentServiceTests()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockSimulatedBankService = new Mock<ISimulatedBankService>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mockMapper = new Mock<IMapper>();
            
            _paymentService = new PaymentService(
                _mockPaymentRepository.Object, 
                _mockSimulatedBankService.Object, 
                _mockTransactionService.Object,
                _mockMapper.Object);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnSuccessResult_WhenBankReturnsSuccessResponse()
        {
            const long ExpectedTransactionID = 9999;
            
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
                .ReturnsAsync(new SimulatedBankResponse(ExpectedTransactionID, TransactionStatus.Success));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(true);

            var actual = await _paymentService.ProcessPayment(paymentDetails);
            
            _mockPaymentRepository.Verify(x => x.SavePaymentDetails(It.IsAny<PaymentDetailsDTO>()), Times.Once);
            _mockSimulatedBankService.Verify(x => x.GetBankResponse(paymentDetails), Times.Once);
            _mockTransactionService.Verify(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()), Times.Once);

            Assert.True(actual.Success);
            Assert.Equal(ExpectedTransactionID, actual.TransactionID);
        }

        [Fact]
        public async Task ProcessPayment_ShouldReturnFailedResult_WhenBankReturnsFailedResponse()
        {
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
                .ReturnsAsync(new SimulatedBankResponse(0, TransactionStatus.Fail));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(true);

            var actual = await _paymentService.ProcessPayment(paymentDetails);
            
            Assert.False(actual.Success);
        }

        [Fact]
        public async Task ProcessPayment_ShouldReturnFailedResult_WhenSavePaymentDetailsOperationThrowsException()
        {
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
            
            _mockPaymentRepository.Setup(x => x.SavePaymentDetails(It.IsAny<PaymentDetailsDTO>())).ThrowsAsync(new Exception());
            _mockSimulatedBankService.Setup(x => x.GetBankResponse(paymentDetails))
                .ReturnsAsync(new SimulatedBankResponse(0, TransactionStatus.Fail));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(true);

            var actual = await _paymentService.ProcessPayment(paymentDetails);
            
            Assert.False(actual.Success);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnFailedResult_WhenSaveTransactionDetailsOperationThrowsException()
        {
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
                .ReturnsAsync(new SimulatedBankResponse(0, TransactionStatus.Fail));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(It.IsAny<TransactionDetails>()))
                .ThrowsAsync(new Exception());

            var actual = await _paymentService.ProcessPayment(paymentDetails);
            
            Assert.False(actual.Success);
        }
    }
}