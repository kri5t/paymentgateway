using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Internal;
using PaymentGatewayCore.Architecture;
using PaymentGatewayCore.BankMock;
using PaymentGatewayCore.Model;
using PaymentGatewayCore.Repository;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayCore.Command
{
    public class CreatePaymentCommand : IRequest<VoidObject>
    {
        public string CardNumber { get; }
        public int ExpiryMonth { get; }
        public int ExpiryYear { get; }
        public int Amount { get; }
        public string Currency { get; }
        public int Cvv { get; }

        /// <summary>
        /// Command to create a payment request
        /// </summary>
        /// <param name="cardNumber"> The card number of the payment request</param>
        /// <param name="expiryMonth"> The expiry month of the card </param>
        /// <param name="expiryYear"> The expiry year of the card </param>
        /// <param name="amount"> The amount that will be charged on the card </param>
        /// <param name="currency"> The currency in which the payment will be processed </param>
        /// <param name="cvv"> The cvv number of the card </param>
        public CreatePaymentCommand(string cardNumber, int expiryMonth, int expiryYear, int amount, string currency, 
            int cvv)
        {
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Amount = amount;
            Currency = currency;
            Cvv = cvv;
        }
    }
    
    [UsedImplicitly]
    public class CreatePaymentCommandHandler : BaseRequestHandler<CreatePaymentCommand, VoidObject>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBank _bank;
        private readonly ISystemClock _systemClock;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IBank bank, ISystemClock systemClock)
        {
            _paymentRepository = paymentRepository;
            _bank = bank;
            _systemClock = systemClock;
        }
        
        public override async Task<VoidObject> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (paymentStatus, bankTransactionUid) = ProcessBankPayment(request);

                await _paymentRepository.CreatePaymentAsync(cancellationToken, request.CardNumber, request.Amount, 
                    request.Currency, paymentStatus, bankTransactionUid, _systemClock.UtcNow);

                await _paymentRepository.SaveAsync(cancellationToken);

                if (paymentStatus != PaymentStatus.PaymentSucceeded)
                    return Error(ErrorCode.UnexpectedError, $"The payment was not a success. It failed with code {paymentStatus}");
            }
            catch (Exception e)
            {
                return Error(ErrorCode.UnexpectedError, e.Message);
            }
            
            return Success();
        }

        private (PaymentStatus paymentStatus, Guid bankTransactionUid) ProcessBankPayment(CreatePaymentCommand request)
        {
            return _bank.CreateTransfer(request.CardNumber, request.ExpiryMonth, request.ExpiryYear, request.Amount,
                request.Currency, request.Cvv);
        }
    }
}