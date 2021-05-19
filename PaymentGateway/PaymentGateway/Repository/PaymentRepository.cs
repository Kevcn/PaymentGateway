using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using PaymentGateway.Configurations;
using PaymentGateway.Repository.DTO;
using Serilog;

namespace PaymentGateway.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MySqlConfig _mySqlConfig;
        private readonly ILogger _logger;
        
        public PaymentRepository(IOptions<MySqlConfig> mySqlConfig, ILogger logger)
        {
            _mySqlConfig = mySqlConfig.Value;
            _logger = logger;
        }
        
        public async Task<int> SavePaymentDetails(PaymentDetailsDTO paymentDetails)
        {
            var insertPaymentDetailsQuery = $@"
                USE {_mySqlConfig.PaymentGatewayDB};
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
                var paymentDetailsID = await _connection.QuerySingleAsync<int>(insertPaymentDetailsQuery,
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
            catch (MySqlException e)
            {
                _logger.Error(e, "Error saving payment details");
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error saving payment details");
                throw;
            }
        }
    }
}