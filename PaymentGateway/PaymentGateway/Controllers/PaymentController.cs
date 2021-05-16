using System.Threading.Tasks;
using AutoMapper;
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

       [HttpPost("Process")]
       public async Task<IActionResult> Process([FromBody] ProcessPaymentRequest processPaymentRequest)
       {
           // TODO: Validation

           var paymentDetails = _mapper.Map<PaymentDetails>(processPaymentRequest);

           var result = await _paymentService.ProcessPayment(paymentDetails);

           if (!result.Success)
           {
               return BadRequest(new FailedResponse("Failed", $"Failed to process payment for card number {paymentDetails.CardNumber}"));
           }

           return Ok(_mapper.Map<SuccessResponse>(result));
       }
    }
}