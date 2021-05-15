using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Models;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

       public PaymentController(IPaymentService paymentService)
       {
           _paymentService = paymentService;
       }

       [HttpPost("Process")]
       public async Task<IActionResult> Process([FromBody] ProcessPaymentRequest processPaymentRequest)
       {
           // TODO: Validation
           
           var paymentDetails = new PaymentDetails
           {
               CardNumber = processPaymentRequest.CardNumber,
               ExpiryMonth = processPaymentRequest.ExpiryMonth,
               ExpiryDate = processPaymentRequest.ExpiryDate,
               CardHolderName = processPaymentRequest.CardHolderName,
               Amount = processPaymentRequest.Amount,
               Currency = processPaymentRequest.Currency,
               CVV = processPaymentRequest.CVV
           };

           var result = await _paymentService.ProcessPayment(paymentDetails);

           if (!result)
           {
               return BadRequest(new {error = $"Failed to save payment details for card number {paymentDetails.CardNumber}"});
           }

           return Ok();
       }
    }
}