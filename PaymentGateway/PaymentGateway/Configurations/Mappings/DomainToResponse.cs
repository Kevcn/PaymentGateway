using AutoMapper;
using PaymentGateway.Contracts.V1.Responses;
using PaymentGateway.Domain;

namespace PaymentGateway.Configurations.Mappings
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<ProcessPaymentResult, SuccessResponse>()
                .ForMember(des => des.Status, opt 
                    => opt.MapFrom(src => src.Success ? "Success" : "Failed"));

            CreateMap<TransactionHistory, TransactionHistoryResponse>();
        }
    }
}