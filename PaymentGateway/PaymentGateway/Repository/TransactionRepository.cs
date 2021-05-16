using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using PaymentGateway.Configurations;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MySqlConfig _mySqlConfig;
        
        public TransactionRepository(IOptions<MySqlConfig> mySqlConfig)
        {
            _mySqlConfig = mySqlConfig.Value;
        }
        
        public async Task<bool> SaveTransactionDetails(TransactionDetailsDTO transactionDetails)
        {
            const string InsertTransactionDetailsQuery = @"
                USE payment_gateway;
                INSERT INTO transaction_details (
                    TransactionID, 
                    Success, 
                    PaymentDetailsID, 
                    CreatedDate) 
                VALUES (
                    @TransactionID, 
                    @Success, 
                    @PaymentDetailsID, 
                    @CreatedDate);";
            
            try
            {
                await using var _connection = new MySqlConnection(_mySqlConfig.ConnectionString);
                var inserted = await _connection.ExecuteAsync(InsertTransactionDetailsQuery,
                    new
                    {
                        transactionDetails.TransactionID,
                        transactionDetails.Success,
                        transactionDetails.PaymentDetailsID,
                        transactionDetails.CreatedDate
                    });

                return inserted > 0;
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