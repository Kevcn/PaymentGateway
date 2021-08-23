using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Contracts;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Services;
using Serilog;

namespace PaymentGateway.Controllers.V1
{
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TransactionController(ITransactionService transactionService, IMapper mapper, ILogger logger)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(ApiRoutes.Transaction.Get)]
        public async Task<IActionResult> Get([FromRoute] long transactionId)
        {
            _logger.Information("Received get transaction request for id {TransactionId}", transactionId);
            var transactionHistory = await _transactionService.GetTransactionHistoryById(transactionId);

            if (transactionHistory == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<TransactionHistoryResponse>(transactionHistory));
        }
    }
}