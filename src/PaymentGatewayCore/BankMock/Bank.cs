using System;
using PaymentGatewayDatabase.Models;
using Serilog;

namespace PaymentGatewayCore.BankMock
{
    public class Bank : IBank
    {
        public (PaymentStatus paymentStatus, Guid bankTransactionUid) CreateTransfer(string cardNumber, int expiryMonth,
            int expiryYear, int amount, string currency, int cvv)
        {
            Log.Information("Dispatching request to the bank");
            var values = Enum.GetValues(typeof(PaymentStatus));
            var random = new Random();
            var randomPaymentStatus = (PaymentStatus)values.GetValue(random.Next(values.Length));
            if(randomPaymentStatus == PaymentStatus.PaymentSucceeded)
                Log.Information("Bank request was accepted");
            else
                Log.Error("There was an error while processing the bank request. Failed with status: {Status}", randomPaymentStatus);
            
            return (randomPaymentStatus, Guid.NewGuid());
        }
    }
}