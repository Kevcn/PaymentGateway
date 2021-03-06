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
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, ICardService cardService, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _cardService = cardService;
            _mapper = mapper;
        }
        
        public async Task<bool> SaveTransactionDetails(TransactionDetails transactionDetails)
        {
            return await _transactionRepository.SaveTransactionDetails(_mapper.Map<TransactionDetailsDTO>(transactionDetails));
        }

        public async Task<TransactionHistory> GetTransactionHistoryById(long transactionID)
        {
            var transactionHistory = await _transactionRepository.GetTransactionHistoryById(transactionID);

            if (transactionHistory == null)
            {
                return null;
            }

            transactionHistory.CardNumber = _cardService.MaskCardNumber(transactionHistory.CardNumber);
            
            return transactionHistory;
        }
    }
}