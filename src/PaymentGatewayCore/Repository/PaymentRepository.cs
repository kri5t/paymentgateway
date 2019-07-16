using PaymentGatewayCore.Aggregate;

namespace PaymentGatewayCore.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public PaymentAggregate CreatePayment(string cardNumber, int expiryMonth, int expiryYear, int amount, string currency,
            string cvv)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IPaymentRepository
    {
        PaymentAggregate CreatePayment(string cardNumber, int expiryMonth, int expiryYear, int amount, string currency, 
            string cvv);
    }
}