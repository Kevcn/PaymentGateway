using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet("GetTransaction")]
        public async Task<IActionResult> GetTransaction([FromRoute] long transactionID)
        {
            // var result = await _transactionService.

            return Accepted();
        }
        
        
        
    }
}