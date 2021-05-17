using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Domain;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

       public PaymentController(IPaymentService paymentService, IMapper mapper)
       {
           _paymentService = paymentService;
           _mapper = mapper;
       }

       [HttpPost("ProcessPayment")]
       public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest processPaymentRequest)
       {
           var paymentDetails = _mapper.Map<PaymentDetails>(processPaymentRequest);

           var result = await _paymentService.ProcessPayment(paymentDetails);
           
           if (!result.Success)
           {
               return StatusCode(StatusCodes.Status500InternalServerError, new FailedResponse
               ("Failed", result.TransactionID, $"Failed to process payment for card {paymentDetails.CardNumber}"));
           }

           return Ok(_mapper.Map<SuccessResponse>(result));
       }
    }
}