using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Configurations.Mappings;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Controllers;
using PaymentGateway.Controllers.V1;
using PaymentGateway.Domain;
using PaymentGateway.Services;
using Serilog;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class PaymentControllerTests
    {
        private readonly PaymentController _paymentController;
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly Mock<IUriService> _mockUriService;
        private readonly Mock<ILogger> _mockLogger;

        public PaymentControllerTests()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _mockUriService = new Mock<IUriService>();
            _mockLogger = new Mock<ILogger>();

            var config = new MapperConfiguration(cfg => 
                cfg.AddProfiles(new List<Profile>{ new RequestToDomain(), new DomainToResponse()}));
            var mapper = config.CreateMapper();
            
            _paymentController = new PaymentController(
                _mockPaymentService.Object,
                _mockUriService.Object,
                mapper,
                _mockLogger.Object);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnSuccessStatus_WhenProcessPaymentSucceeds()
        {
            var expectedStatusCode = 201;
            var expectedTransactionID = 9900;
            var expectedStatus = "Success";
            
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

            var paymentProcessresult = new ProcessPaymentResult(true, expectedTransactionID);
            
            _mockPaymentService.Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
                .ReturnsAsync(paymentProcessresult);
            _mockUriService.Setup(x => x.GetTransactionUri(expectedTransactionID)).Returns(new Uri("localhost:111"));

            var actual = (ObjectResult) await _paymentController.ProcessPayment(processPaymentRequest);
            var actualResponse = actual.Value as ProcessPaymentResponse;
            
            Assert.Equal(expectedStatusCode, actual.StatusCode);
            Assert.Equal(expectedStatus, actualResponse.Status);
            Assert.Equal(expectedTransactionID, actualResponse.TransactionID);
        }
        
        [Fact]
        public async Task ProcessPayment_ShouldReturnFailedStatus_WhenProcessPaymentFails()
        {
            var expectedStatusCode = 201;
            var expectedTransactionID = 9901;
            var expectedStatus = "Failed";
            
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

            var paymentProcessresult = new ProcessPaymentResult(false, expectedTransactionID);
            
            _mockPaymentService.Setup(x => x.ProcessPayment(It.IsAny<PaymentDetails>()))
                .ReturnsAsync(paymentProcessresult);
            _mockUriService.Setup(x => x.GetTransactionUri(expectedTransactionID)).Returns(new Uri("localhost:111"));

            var actual = (ObjectResult) await _paymentController.ProcessPayment(processPaymentRequest);
            var actualResponse = actual.Value as ProcessPaymentResponse;
            
            Assert.Equal(expectedStatusCode, actual.StatusCode);
            Assert.Equal(expectedStatus, actualResponse.Status);
            Assert.Equal(expectedTransactionID, actualResponse.TransactionID);
        }
    }
}