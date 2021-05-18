using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet("transaction/{transactionID}")]
        public async Task<IActionResult> GetTransaction([FromRoute] long transactionID)
        {
            var transactionHistory = await _transactionService.GetTransactionHistoryById(transactionID);

            if (transactionHistory == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<TransactionHistoryResponse>(transactionHistory));
        }
    }
}