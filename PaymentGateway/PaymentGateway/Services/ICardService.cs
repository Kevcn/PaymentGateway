namespace PaymentGateway.Services
{
    public interface ICardService
    {
        string MaskCardNumber(string cardNumber);
    }
}