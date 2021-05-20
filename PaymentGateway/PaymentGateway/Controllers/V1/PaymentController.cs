using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Contracts;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Domain;
using PaymentGateway.Services;
using Serilog;

namespace PaymentGateway.Controllers.V1
{
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentController(IPaymentService paymentService, IUriService uriService, IMapper mapper, ILogger logger)
       {
           _paymentService = paymentService;
           _uriService = uriService;
           _mapper = mapper;
           _logger = logger;
       }

       [HttpPost(ApiRoutes.Payment.Process)]
       public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest processPaymentRequest)
       {
           _logger.Information($"Received process payment request for card {processPaymentRequest.CardNumber}");
           var paymentDetails = _mapper.Map<PaymentDetails>(processPaymentRequest);

           var result = await _paymentService.ProcessPayment(paymentDetails);
           
           var locationUri = _uriService.GetTransactionUri(result.TransactionID);
           return Created(locationUri, _mapper.Map<ProcessPaymentResponse>(result));
       }
    }
}