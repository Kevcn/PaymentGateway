using System;
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
        private const string ConnectionString = "Server=localhost;;Uid=rw_user;Pwd=Warrington4";

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
            var paymentDetails = new PaymentDetailsDTO
            {
                CardNumber = "1234123412341234",
                ExpiryMonth = 5,
                ExpiryDate = 23,
                CardHolderName = "Y LI",
                Amount = 18.99M,
                Currency = "GBP",
                CVV = "111"
            };

            await _paymentRepository.SavePaymentDetails(paymentDetails);

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

            var command = new MySqlCommand(getPaymentDetails, connection);


        }
        

    }
}