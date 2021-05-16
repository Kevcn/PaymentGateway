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
        private readonly Mock<IMapper> _mapper;
        
        public PaymentServiceTests()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockSimulatedBankService = new Mock<ISimulatedBankService>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mapper = new Mock<IMapper>();
            
            _paymentService = new PaymentService(
                _mockPaymentRepository.Object, 
                _mockSimulatedBankService.Object, 
                _mockTransactionService.Object,
                _mapper.Object);
        }

        // Should return expected transaction details when all operations are successful
        // Should return failed when bank response is failed
        // should return failed when save payment details operation throws an exception
        // Should return failed when save transaction operation throws an exception
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnSuccessResult_WhenAllOperationsAreSuccessful()
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
            
            var paymentDetailsDTO = new PaymentDetailsDTO
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };

            _mockPaymentRepository.Setup(x => x.SavePaymentDetails(paymentDetailsDTO)).ReturnsAsync(123);
            _mockSimulatedBankService.Setup(x => x.GetBankResponse(new PaymentDetails()))
                .ReturnsAsync(new SimulatedBankResponse(ExpectedTransactionID, TransactionStatus.Success));
            _mockTransactionService
                .Setup(x => x.SaveTransactionDetails(new TransactionDetails(ExpectedTransactionID, true, 123)))
                .ReturnsAsync(true);

            var actual = await _paymentService.ProcessPayment(paymentDetails);
            
            Assert.True(actual.Success);
            Assert.Equal(ExpectedTransactionID, actual.TransactionID);
        }
    }
}