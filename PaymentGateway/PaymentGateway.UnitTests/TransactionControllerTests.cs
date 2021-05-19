using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using PaymentGateway.Configurations.Mappings;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Controllers;
using PaymentGateway.Domain;
using PaymentGateway.Services;
using Serilog;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class TransactionControllerTests
    {
        private readonly TransactionController _transactionController;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<ILogger> _mockLogger;

        public TransactionControllerTests()
        {
            _mockTransactionService = new Mock<ITransactionService>();
            _mockLogger = new Mock<ILogger>();
            
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponse>());
            var mapper = config.CreateMapper();

            _transactionController = new TransactionController(
                _mockTransactionService.Object,
                mapper,
                _mockLogger.Object);
        }
        
        [Fact]
        public async Task GetTransaction_ShouldReturnTransactionHistory_WhenFoundById()
        {
            var transactionId = 29292;

            var expectedTransactionHistory = new TransactionHistory
            {
                CardNumber = "123412******1234",
                CardHolderName = "K Li",
                Amount = 999.8m,
                Currency = "GBP",
                Success = true,
                CreatedDate = new DateTime(2021, 5, 17, 22, 17, 00)
            };
            
            _mockTransactionService.Setup(x => x.GetTransactionHistoryById(transactionId)).ReturnsAsync(expectedTransactionHistory);

            var actual = (OkObjectResult) await _transactionController.GetTransaction(transactionId);
            var actualResponse = actual.Value as TransactionHistoryResponse;
            
            Assert.Equal(expectedTransactionHistory.CardNumber, actualResponse.CardNumber);
            Assert.Equal(expectedTransactionHistory.CardHolderName, actualResponse.CardHolderName);
            Assert.Equal(expectedTransactionHistory.Amount, actualResponse.Amount);
            Assert.Equal(expectedTransactionHistory.Currency, actualResponse.Currency);
            Assert.Equal(expectedTransactionHistory.Success, actualResponse.Success);
            Assert.Equal(expectedTransactionHistory.CreatedDate, actualResponse.CreatedDate);
        }
        
        [Fact]
        public async Task GetTransaction_ShouldReturnCorrectStatusCode_WhenNotFoundById()
        {
            var transactionId = 29293;
            var expectedStatusCode = 404;

            _mockTransactionService.Setup(x => x.GetTransactionHistoryById(transactionId)).
                ReturnsAsync(() => null);

            var actual = (IStatusCodeActionResult) await _transactionController.GetTransaction(transactionId);
            var actualStatusCode = actual.StatusCode;
            
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }
    }
}