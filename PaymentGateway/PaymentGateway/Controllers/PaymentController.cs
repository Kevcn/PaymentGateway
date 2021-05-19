using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Domain;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentController(IPaymentService paymentService, IMapper mapper, ILogger logger)
       {
           _paymentService = paymentService;
           _mapper = mapper;
           _logger = logger;
       }

       [HttpPost("processPayment")]
       public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest processPaymentRequest)
       {
           _logger.Information($"Received process payment request for card {processPaymentRequest.CardNumber}");
           var paymentDetails = _mapper.Map<PaymentDetails>(processPaymentRequest);

           var result = await _paymentService.ProcessPayment(paymentDetails);
           
           return Ok(_mapper.Map<ProcessPaymentResponse>(result));
       }
    }
}