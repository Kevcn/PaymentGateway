using AutoMapper;
using PaymentGateway.Contracts.V1.Requests;
using PaymentGateway.Domain;

namespace PaymentGateway.Configurations.Mappings
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<ProcessPaymentRequest, PaymentDetails>();
        }
    }
}