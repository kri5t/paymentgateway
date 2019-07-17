using System;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayCore.BankMock
{
    public interface IBank
    {
        (PaymentStatus paymentStatus, Guid bankTransactionUid) CreateTransfer(string cardNumber, int expiryMonth,
            int expiryYear, int amount, string currency, int cvv);
    }
}