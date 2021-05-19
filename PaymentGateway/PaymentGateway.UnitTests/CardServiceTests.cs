using PaymentGateway.Services;
using Xunit;

namespace PaymentGateway.UnitTests
{
    public class CardServiceTests
    {
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _cardService = new CardService();
        }
        
        [Fact]
        public void MaskCardNumber_ShouldMaskCardNumber_WhenGivenCardNumber()
        {
            var expectedMaskedCardNumber = "11112******34444";
            var cardNumber = "1111222233334444";
            
            var actual = _cardService.MaskCardNumber(cardNumber);

            Assert.Equal(expectedMaskedCardNumber, actual);
        }
    }
}