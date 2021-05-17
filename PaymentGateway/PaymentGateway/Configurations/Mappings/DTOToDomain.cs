using AutoMapper;
using PaymentGateway.Domain;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Configurations.Mappings
{
    public class DTOToDomain : Profile
    {
        public DTOToDomain()
        {
            CreateMap<TransactionHistoryDTO, TransactionHistory>();
        }
    }
}