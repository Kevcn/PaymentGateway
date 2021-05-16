using AutoMapper;
using PaymentGateway.Domain;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Configurations.Mappings
{
    public class DomainToDTO : Profile
    {
        public DomainToDTO()
        {
            CreateMap<PaymentDetails, PaymentDetailsDTO>();
            CreateMap<TransactionDetails, TransactionDetailsDTO>();
        }
    }
}