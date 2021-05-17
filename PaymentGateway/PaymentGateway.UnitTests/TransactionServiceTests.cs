using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
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
        private readonly Mock<IMapper> _mockMapper;

        public TransactionServiceTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockCardService = new Mock<ICardService>();
            _mockMapper = new Mock<IMapper>();

            _transactionService = new TransactionService(
                _mockTransactionRepository.Object, 
                _mockCardService.Object,
                _mockMapper.Object);
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
            const long TransactionID = 31215;
            const string ExpectedCardNumber = "123412******1234";

            var expectedTransactionHistory = new TransactionHistory
            {
                CardNumber = "1234123412341234",
                CardHolderName = "K Li",
                Amount = 999.8m,
                Currency = "GBP",
                Success = true,
                CreatedDate = new DateTime(2021, 5, 17, 22, 17, 00)
            };

            _mockTransactionRepository.Setup(x => x.GetTransactionHistoryById(TransactionID))
                .ReturnsAsync(expectedTransactionHistory);
            _mockCardService.Setup(x => x.MaskCardNumber(expectedTransactionHistory.CardNumber))
                .Returns(ExpectedCardNumber);

            var actual = await _transactionService.GetTransactionHistoryById(TransactionID);

            Assert.Equal(ExpectedCardNumber, actual.CardNumber);
            Assert.Equal(expectedTransactionHistory.CardHolderName, actual.CardHolderName);
            Assert.Equal(expectedTransactionHistory.Amount, actual.Amount);
            Assert.Equal(expectedTransactionHistory.Currency, actual.Currency);
            Assert.Equal(expectedTransactionHistory.Success, actual.Success);
            Assert.Equal(expectedTransactionHistory.CreatedDate, actual.CreatedDate);
        }
        
        [Fact]
        public async Task GetTransactionHistoryById_ShouldReturnNull_WhenTransactionNotFound()
        {
            const long TransactionID = 31215;

            _mockTransactionRepository.Setup(x => x.GetTransactionHistoryById(TransactionID))
                .ReturnsAsync(() => null);

            var actual = await _transactionService.GetTransactionHistoryById(TransactionID);
            
            Assert.Null(actual);
        }
    }
}