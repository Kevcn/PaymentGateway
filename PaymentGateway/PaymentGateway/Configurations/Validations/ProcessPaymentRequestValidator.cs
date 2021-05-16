using FluentValidation;
using PaymentGateway.Contracts.V1.Requests;

namespace PaymentGateway.Configurations.Validations
{
    public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
    {
        public ProcessPaymentRequestValidator()
        {
            RuleFor(x => x)
                .NotEmpty();
            
            // Expecting exactly 16 digits
            RuleFor(x => x.CardNumber)
                .Matches("^[0-9]{16}$");
            
            // Expecting exactly 3 capital letters 
            RuleFor(x => x.Currency)
                .Matches("^[A-Z]{3}$");
            
            RuleFor(x => x.CVV)
                .Length(3);
        }
    }
}