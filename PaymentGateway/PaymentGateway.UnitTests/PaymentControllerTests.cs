using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Controllers;
using PaymentGateway.Domain;
using PaymentGateway.Services;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class PaymentControllerTests
    {
        private readonly PaymentController _paymentController;
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly Mock<IMapper> _mockMapper;

        public PaymentControllerTests()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _mockMapper = new Mock<IMapper>();
            
            _paymentController = new PaymentController(
                _mockPaymentService.Object,
                _mockMapper.Object);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnSuccessResponse_WhenProcessPaymentSucceeds()
        {
            const long ExpectedTransactionID = 9900;

            var expectedResponse = new SuccessResponse
            {
                Status = "Success",
                TransactionID = ExpectedTransactionID
            };
            
            var processPaymentRequest = new ProcessPaymentRequest
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };

            var paymentProcessresult = new ProcessPaymentResult(true, ExpectedTransactionID);
            
            _mockPaymentService.Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
                .ReturnsAsync(paymentProcessresult);
            _mockMapper.Setup(x => x.Map<SuccessResponse>(paymentProcessresult)).Returns(expectedResponse);

            var actual = (OkObjectResult) await _paymentController.ProcessPayment(processPaymentRequest);
            var actualResponse = actual.Value as SuccessResponse;
            
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.TransactionID, actualResponse.TransactionID);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnFailedResponse_WhenProcessPaymentFails()
        {
            const long ExpectedTransactionID = 9901;

            var expectedResponse = new SuccessResponse
            {
                Status = "Failed",
                TransactionID = ExpectedTransactionID
            };
            
            var processPaymentRequest = new ProcessPaymentRequest
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };

            var paymentProcessresult = new ProcessPaymentResult(false, ExpectedTransactionID);
            
            _mockPaymentService.Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
                .ReturnsAsync(paymentProcessresult);
            _mockMapper.Setup(x => x.Map<SuccessResponse>(paymentProcessresult)).Returns(expectedResponse);

            var actual = (OkObjectResult) await _paymentController.ProcessPayment(processPaymentRequest);
            var actualResponse = actual.Value as SuccessResponse;
            
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.TransactionID, actualResponse.TransactionID);
        }
    }
}