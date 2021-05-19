using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using PaymentGateway.Configurations;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    public class PaymentRepositoryTests : MySqlTestBase
    {
        private const string TestDatabaseName = "payment_gateway_test";
        private const string ConnectionString = "Server=localhost;Uid=rw_user;Pwd=Warrington4";

        private readonly PaymentRepository _paymentRepository;
        
        public PaymentRepositoryTests() : base(ConnectionString, TestDatabaseName + + new Random().Next(0, int.MaxValue))
        {
            _paymentRepository = new PaymentRepository(Options.Create(new MySqlConfig
            {
                ConnectionString = ConnectionString,
                PaymentGatewayDB = _databaseName
            }));
        }

        protected override string DatabaseSeed => MySqlTables.CreatePaymentDetails;

        [Fact]
        public async Task SavePaymentDetails_ShouldSavePayment_WhenGivenValidDetails()
        {
            var expectedPaymentDetails = new PaymentDetailsDTO
            {
                CardNumber = "1234123412341266",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };

            await _paymentRepository.SavePaymentDetails(expectedPaymentDetails);

            var getPaymentDetails = $@"SELECT 
	                                    CardNumber, 
	                                    ExpiryMonth, 
	                                    ExpiryDate, 
	                                    CardHolderName, 
	                                    Amount, 
	                                    Currency, 
	                                    CVV
                                     FROM {_databaseName}.payment_details;";

            await using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();

            var dt = new DataTable("PaymentDetails");

            await using var command = new MySqlCommand(getPaymentDetails, connection);
            var da = new MySqlDataAdapter(command);
            da.Fill(dt);

            var actual = (from DataRow row in dt.Rows
            select new PaymentDetailsDTO
            {
                CardNumber = Convert.ToString(row[dt.Columns["CardNumber"].Ordinal]),
                ExpiryMonth = Convert.ToInt32(row[dt.Columns["ExpiryMonth"].Ordinal]),
                ExpiryDate = Convert.ToInt32(row[dt.Columns["ExpiryDate"].Ordinal]),
                CardHolderName = Convert.ToString(row[dt.Columns["CardHolderName"].Ordinal]),
                Amount = Convert.ToDecimal(row[dt.Columns["Amount"].Ordinal]),
                Currency = Convert.ToString(row[dt.Columns["Currency"].Ordinal]),
                CVV = Convert.ToString(row[dt.Columns["CVV"].Ordinal])
            }).ToList().FirstOrDefault();
            
            Assert.Equal(expectedPaymentDetails.CardNumber, actual.CardNumber);
            Assert.Equal(expectedPaymentDetails.ExpiryMonth, actual.ExpiryMonth);
            Assert.Equal(expectedPaymentDetails.ExpiryDate, actual.ExpiryDate);
            Assert.Equal(expectedPaymentDetails.CardHolderName, actual.CardHolderName);
            Assert.Equal(expectedPaymentDetails.Amount, actual.Amount);
            Assert.Equal(expectedPaymentDetails.Currency, actual.Currency);
            Assert.Equal(expectedPaymentDetails.CVV, actual.CVV); 
        }
    }
}