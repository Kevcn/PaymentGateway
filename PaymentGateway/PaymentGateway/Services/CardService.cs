namespace PaymentGateway.Services
{
    public class CardService : ICardService
    {
        public string MaskCardNumber(string cardNumber)
        {
            return cardNumber.Remove(5, 6).Insert(5, "******");
        }
    }
}