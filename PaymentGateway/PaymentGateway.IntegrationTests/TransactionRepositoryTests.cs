using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using MySql.Data.MySqlClient;
using PaymentGateway.Configurations;
using PaymentGateway.Configurations.Mappings;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using Serilog;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    public class TransactionRepositoryTests : MySqlTestBase
    {
        private const string TestDatabaseName = "payment_gateway_test";
        private const string ConnectionString = "Server=localhost;Uid=rw_user;Pwd=Warrington4";
        
        private readonly TransactionRepository _transactionRepository;
        private readonly Mock<ILogger> _mockLogger;

        public TransactionRepositoryTests() : base(ConnectionString, TestDatabaseName + + new Random().Next(0, int.MaxValue))
        {
            _mockLogger = new Mock<ILogger>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<DTOToDomain>());
            var mapper = config.CreateMapper();

            _transactionRepository = new TransactionRepository(Options.Create(new MySqlConfig
            {
                ConnectionString = ConnectionString,
                PaymentGatewayDB = _databaseName
            }), mapper, _mockLogger.Object);
        }

        protected override string DatabaseSeed => MySqlTables.CreatePaymentDetails +
                                                  MySqlTables.CreateTransactionDetails;

        [Fact]
        public async Task SaveTransactionDetails_ShouldSavePayment_WhenGivenValidDetails()
        {
            var expectedTransactionDetails = new TransactionDetailsDTO
            {
                TransactionID = 8765,
                Success = true,
                PaymentDetailsID = 221,
                CreatedDate = new DateTime(2021, 5, 19, 22, 17, 00)
            };

            await _transactionRepository.SaveTransactionDetails(expectedTransactionDetails);
            
            var getTransactionDetails = $@"SELECT 
	                                        TransactionID,
                                            Success,
                                            PaymentDetailsID,
                                            CreatedDate
                                         FROM {_databaseName}.transaction_details;";

            await using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();

            var dt = new DataTable("TransactionDetails");

            await using var command = new MySqlCommand(getTransactionDetails, connection);
            var da = new MySqlDataAdapter(command);
            da.Fill(dt);

            var actual = (from DataRow row in dt.Rows
                select new TransactionDetailsDTO
                {
                    TransactionID = Convert.ToInt64(row[dt.Columns["TransactionID"].Ordinal]),
                    Success = Convert.ToBoolean(row[dt.Columns["Success"].Ordinal]),
                    PaymentDetailsID = Convert.ToInt32(row[dt.Columns["PaymentDetailsID"].Ordinal]),
                    CreatedDate = Convert.ToDateTime(row[dt.Columns["CreatedDate"].Ordinal])
                }).ToList().FirstOrDefault();

            Assert.Equal(expectedTransactionDetails.TransactionID, actual.TransactionID);
            Assert.Equal(expectedTransactionDetails.Success, actual.Success);
            Assert.Equal(expectedTransactionDetails.PaymentDetailsID, actual.PaymentDetailsID);
            Assert.Equal(expectedTransactionDetails.CreatedDate, actual.CreatedDate);
        }

        [Fact]
        public async Task GetTransactionHistoryById_ShouldGetTransactionHistory_WhenTransactionHistoryExists()
        {
            var transactionID = 2121;

            var expectedCardNumber = "1234123412341266";
            var expectedCardHolderName = "John J";
            var expectedAmount = 734.7m;
            var expectedCurrency = "GBP";
            var expectedSuccess = true;
            var expectedCreatedDate = new DateTime(2021, 5, 19, 22, 17, 00);

            var paymentDetails = new PaymentDetailsDTO
            {
                CardNumber = expectedCardNumber,
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = expectedCardHolderName,
                Amount = expectedAmount,
                Currency = expectedCurrency,
                CVV = "111"
            };

            var transactionDetails = new TransactionDetailsDTO
            {
                TransactionID = transactionID,
                Success = expectedSuccess,
                PaymentDetailsID = 1,
                CreatedDate = expectedCreatedDate
            };

            var insertDetails = $@"INSERT INTO {_databaseName}.payment_details 
                                    (`CardNumber`,
                                    `ExpiryMonth`,
                                    `ExpiryDate`,
                                    `CardHolderName`,
                                    `Amount`,
                                    `Currency`,
                                    `CVV`)
                                  VALUES 
                                    (@CardNumber, 
                                     @ExpiryMonth, 
                                     @ExpiryDate, 
                                     @CardHolderName,
                                     @Amount,
                                     @Currency,
                                     @CVV);
                                 INSERT INTO {_databaseName}.transaction_details
                                    (`TransactionID`,
                                    `Success`,
                                    `PaymentDetailsID`,
                                    `CreatedDate`)
                                 VALUES
                                    (@TransactionID, 
                                     @Success, 
                                     @PaymentDetailsID, 
                                     @CreatedDate);";
                                                        
            await using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(insertDetails, connection);
            command.Parameters.AddWithValue("@CardNumber", paymentDetails.CardNumber);
            command.Parameters.AddWithValue("@ExpiryMonth", paymentDetails.ExpiryMonth);
            command.Parameters.AddWithValue("@ExpiryDate", paymentDetails.ExpiryDate);
            command.Parameters.AddWithValue("@CardHolderName", paymentDetails.CardHolderName);
            command.Parameters.AddWithValue("@Amount", paymentDetails.Amount);
            command.Parameters.AddWithValue("@Currency", paymentDetails.Currency);
            command.Parameters.AddWithValue("@CVV", paymentDetails.CVV);
            command.Parameters.AddWithValue("@TransactionID", transactionDetails.TransactionID);
            command.Parameters.AddWithValue("@Success", transactionDetails.Success);
            command.Parameters.AddWithValue("@PaymentDetailsID", transactionDetails.PaymentDetailsID);
            command.Parameters.AddWithValue("@CreatedDate", transactionDetails.CreatedDate);
            
            command.ExecuteNonQuery();
            
            var actual = await _transactionRepository.GetTransactionHistoryById(transactionID);
            
            Assert.Equal(expectedCardNumber, actual.CardNumber);
            Assert.Equal(expectedCardHolderName, actual.CardHolderName);
            Assert.Equal(expectedAmount, actual.Amount);
            Assert.Equal(expectedSuccess, actual.Success);
            Assert.Equal(expectedCreatedDate, actual.CreatedDate);
        }
    }
}