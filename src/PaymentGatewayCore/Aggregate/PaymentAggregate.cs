using System;
using System.Linq;
using System.Runtime.CompilerServices;
using PaymentGatewayDatabase.Models;

[assembly:InternalsVisibleTo("PaymentGatewayTest")]
namespace PaymentGatewayCore.Aggregate
{
    public class PaymentAggregate
    {
        internal readonly Payment State;
        
        /// <summary>
        /// Hydrate the aggregate with an already existing payment
        /// </summary>
        /// <param name="state"> The payment for the aggregate </param>
        public PaymentAggregate(Payment state)
        {
            State = state;
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        /// <param name="cardNumber"> The card number of the payment request</param>
        /// <param name="amount"> The amount that will be charged on the card </param>
        /// <param name="currency"> The currency in which the payment will be processed </param>
        /// <param name="paymentStatus"> Status of the payment done with the bank </param>
        /// <param name="bankTransactionUid"> The uid of the transaction </param>
        /// <param name="createdDateUtc"> When the transaction was done </param>
        public PaymentAggregate(string cardNumber, int amount, string currency, PaymentStatus paymentStatus, Guid bankTransactionUid, DateTimeOffset createdDateUtc)
        {
            State = new Payment();
            AddCardNumber(cardNumber);
            AddAmount(amount);
            AddCurrency(currency);
            AddBankTransactionUid(bankTransactionUid);
            State.Uid = Guid.NewGuid();
            State.PaymentStatus = paymentStatus;
            State.CreatedDateUtc = createdDateUtc;
        }

        /// <summary>
        /// Add bank transaction uid to track the transaction at the bank
        /// </summary>
        /// <param name="bankTransactionUid"> Uid is issued by the bank </param>
        /// <exception cref="ArgumentNullException"> If the uid is default we assume this is not a valid transaction </exception>
        private void AddBankTransactionUid(Guid bankTransactionUid)
        {
            if(bankTransactionUid == default(Guid))
                throw new ArgumentNullException(nameof(bankTransactionUid), "Bank transaction uid cannot be default(null)");

            State.BankTransactionUid = bankTransactionUid;
        }

        /// <summary>
        /// Add currency for the charge
        /// </summary>
        /// <param name="currency"> The currency </param>
        /// <exception cref="ArgumentException"> If the currency is empty or null </exception>
        private void AddCurrency(string currency)
        {
            if(string.IsNullOrWhiteSpace(currency))
                throw new ArgumentNullException(nameof(currency), "Currency cannot be null or empty");

            State.Currency = currency;
        }

        /// <summary>
        /// Add amount for the charge
        /// </summary>
        /// <param name="amount"> The amount charged </param>
        /// <exception cref="ArgumentOutOfRangeException"> If the amount is equal to or less than zero </exception>
        private void AddAmount(int amount)
        {
            if(amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be zero or less");

            State.Amount = amount;
        }

        /// <summary>
        /// Add card number of the payment registration
        /// </summary>
        /// <param name="cardNumber"> The card number </param>
        /// <exception cref="ArgumentNullException"> If card number is not set </exception>
        private void AddCardNumber(string cardNumber)
        {
            if(string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentNullException(nameof(cardNumber), "Card number cannot be null");
            
            State.ObfuscatedCardNumber = ObfuscateCardNumber(cardNumber);
        }

        /// <summary>
        /// Assuming we do not save the card number for our customer.
        ///
        /// When the transaction is done. We just save the last 4 digits.
        /// </summary>
        /// <param name="cardNumber"> The card number </param>
        /// <returns> Obfuscated card number </returns>
        /// <exception cref="ArgumentOutOfRangeException"> Raised if the card number is not exactly 16 characters </exception>
        private string ObfuscateCardNumber(string cardNumber)
        {
            if(cardNumber.Length != 16)
                throw new ArgumentOutOfRangeException(nameof(cardNumber), "Card number is not the right format");

            var lastFourDigits = new string(cardNumber.Skip(12).Take(4).ToArray());
            return $"xxxxxxxxxxxx{lastFourDigits}";
        }
    }
}