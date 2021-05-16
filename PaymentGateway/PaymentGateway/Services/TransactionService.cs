using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.Domain;
using PaymentGateway.Repository;
using PaymentGateway.Repository.DTO;

namespace PaymentGateway.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        
        public async Task<bool> SaveTransactionDetails(TransactionDetails transactionDetails)
        {
            return await _transactionRepository.SaveTransactionDetails(_mapper.Map<TransactionDetailsDTO>(transactionDetails));
        }
    }
}