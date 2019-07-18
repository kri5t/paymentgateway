using System;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayCore.BankMock
{
    public class Bank : IBank
    {
        public (PaymentStatus paymentStatus, Guid bankTransactionUid) CreateTransfer(string cardNumber, int expiryMonth,
            int expiryYear, int amount, string currency, int cvv)
        {
            var values = Enum.GetValues(typeof(PaymentStatus));
            var random = new Random();
            var randomPaymentStatus = (PaymentStatus)values.GetValue(random.Next(values.Length));
            return (randomPaymentStatus, Guid.NewGuid());
        }
    }
}