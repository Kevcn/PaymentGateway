using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using PaymentGateway.Configurations;
using PaymentGateway.Domain;
using PaymentGateway.Repository.DTO;
using Serilog;

namespace PaymentGateway.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MySqlConfig _mySqlConfig;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TransactionRepository(IOptions<MySqlConfig> mySqlConfig, IMapper mapper, ILogger logger)
        {
            _mySqlConfig = mySqlConfig.Value;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<bool> SaveTransactionDetails(TransactionDetailsDTO transactionDetails)
        {
            var insertTransactionDetailsQuery = $@"
                USE {_mySqlConfig.PaymentGatewayDB};
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
                var inserted = await _connection.ExecuteAsync(insertTransactionDetailsQuery,
                    new
                    {
                        transactionDetails.TransactionID,
                        transactionDetails.Success,
                        transactionDetails.PaymentDetailsID,
                        transactionDetails.CreatedDate
                    });

                return inserted > 0;
            }
            catch (MySqlException e)
            {
                _logger.Error(e, "Error saving transaction details");
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error saving transaction details");
                throw;
            }
        }

        public async Task<TransactionHistory> GetTransactionHistoryById(long transactionID)
        {
            var getTransactionHistoryQuery = $@"
                USE {_mySqlConfig.PaymentGatewayDB};
                SELECT 
                    CardNumber,
                    CardHolderName,
                    Amount,
                    Currency,
                    Success,
                    CreatedDate
                FROM transaction_details td
                JOIN payment_details pd ON td.PaymentDetailsID = pd.ID
                WHERE TransactionID = @transactionID";

            try
            {
                await using var _connection = new MySqlConnection(_mySqlConfig.ConnectionString);
                var transactionHistory = await _connection.QueryAsync<TransactionHistoryDTO>(getTransactionHistoryQuery, 
                    new
                    {
                        transactionID
                    });

                return _mapper.Map<TransactionHistory>(transactionHistory.FirstOrDefault());
            }
            catch (MySqlException e)
            {
                _logger.Error(e, "Error getting transaction history by id");
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error getting transaction history by id");
                throw;
            }
        }
    }
}