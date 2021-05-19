using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PaymentGateway.Configurations.Mappings;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using PaymentGateway.Services;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly TransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<ICardService> _mockCardService;

        public TransactionServiceTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockCardService = new Mock<ICardService>();
            
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainToDTO>());
            var mapper = config.CreateMapper();

            _transactionService = new TransactionService(
                _mockTransactionRepository.Object, 
                _mockCardService.Object,
                mapper);
        }

        [Fact]
        public async Task SaveTransactionDetails_ShouldReturnTrue_WhenTransactionDetailsAreSaved()
        {
            var transactionDetails = new TransactionDetails(932131, true, 103);

            _mockTransactionRepository.Setup(x =>
                x.SaveTransactionDetails(It.IsAny<TransactionDetailsDTO>())).ReturnsAsync(true);

            var actual = await _transactionService.SaveTransactionDetails(transactionDetails);
            
            Assert.True(actual);
        }

        [Fact]
        public async Task SaveTransactionDetails_ShouldReturnFalse_WhenTransactionDetailsAreNotSaved()
        {
            var transactionDetails = new TransactionDetails(932132, true, 104);

            _mockTransactionRepository.Setup(x =>
                x.SaveTransactionDetails(It.IsAny<TransactionDetailsDTO>())).ReturnsAsync(false);

            var actual = await _transactionService.SaveTransactionDetails(transactionDetails);
            
            Assert.False(actual);
        }

        [Fact]
        public async Task GetTransactionHistoryById_ShouldReturnTransactionHistory_WhenTransactionFound()
        {
            var transactionId = 31215;
            var expectedCardNumber = "123412******1234";

            var expectedTransactionHistory = new TransactionHistory
            {
                CardNumber = "1234123412341234",
                CardHolderName = "K Li",
                Amount = 999.8m,
                Currency = "GBP",
                Success = true,
                CreatedDate = new DateTime(2021, 5, 17, 22, 17, 00)
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionHistoryById(transactionId))
                .ReturnsAsync(expectedTransactionHistory);
            _mockCardService.Setup(x => x.MaskCardNumber(expectedTransactionHistory.CardNumber))
                .Returns(expectedCardNumber);

            var actual = await _transactionService.GetTransactionHistoryById(transactionId);

            Assert.Equal(expectedCardNumber, actual.CardNumber);
            Assert.Equal(expectedTransactionHistory.CardHolderName, actual.CardHolderName);
            Assert.Equal(expectedTransactionHistory.Amount, actual.Amount);
            Assert.Equal(expectedTransactionHistory.Currency, actual.Currency);
            Assert.Equal(expectedTransactionHistory.Success, actual.Success);
            Assert.Equal(expectedTransactionHistory.CreatedDate, actual.CreatedDate);
        }
        
        [Fact]
        public async Task GetTransactionHistoryById_ShouldReturnNull_WhenTransactionNotFound()
        {
            var transactionId = 31215;

            _mockTransactionRepository.Setup(x => x.GetTransactionHistoryById(transactionId))
                .ReturnsAsync(() => null);

            var actual = await _transactionService.GetTransactionHistoryById(transactionId);
            
            Assert.Null(actual);
        }
    }
}