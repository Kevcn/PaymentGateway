using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using PaymentGateway.Configurations;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MySqlConfig _mySqlConfig;
        
        public PaymentRepository(IOptions<MySqlConfig> mySqlConfig)
        {
            _mySqlConfig = mySqlConfig.Value;
        }
        
        public async Task<int> SavePaymentDetails(PaymentDetailsDTO paymentDetails)
        {
            const string InsertPaymentDetailsQuery = @"
                USE payment_gateway;
                INSERT INTO payment_details (
                    CardNumber, 
                    ExpiryMonth, 
                    ExpiryDate, 
                    CardHolderName, 
                    Amount, 
                    Currency, 
                    CVV) 
                VALUES (
                    @CardNumber, 
                    @ExpiryMonth, 
                    @ExpiryDate, 
                    @CardHolderName, 
                    @Amount, 
                    @Currency, 
                    @CVV);
                SELECT LAST_INSERT_ID()";
            
            try
            {
                await using var _connection = new MySqlConnection(_mySqlConfig.ConnectionString);
                var paymentDetailsID = await _connection.QuerySingleAsync<int>(InsertPaymentDetailsQuery,
                    new
                    {
                        paymentDetails.CardNumber,
                        paymentDetails.ExpiryMonth,
                        paymentDetails.ExpiryDate,
                        paymentDetails.CardHolderName,
                        paymentDetails.Amount,
                        paymentDetails.Currency,
                        paymentDetails.CVV
                    });

                return paymentDetailsID;
            }
            catch (MySqlException exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
            catch (Exception exception)
            {
                // TODO: log expection
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}