using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PaymentGatewayCore.Architecture;
using PaymentGatewayCore.Model;

namespace PaymentGatewayCore.Command
{
    public class CreatePaymentCommand : IRequest<CreatePaymentCommandResponse>
    {
        public string CardNumber { get; }
        public int ExpiryMonth { get; }
        public int ExpiryYear { get; }
        public int Amount { get; }
        public string Currency { get; }
        public string Cvv { get; }

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
            string cvv)
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
    public class CreatePaymentCommandHandler : BaseRequestHandler<CreatePaymentCommand, CreatePaymentCommandResponse>
    {
        public CreatePaymentCommandHandler()
        {
        }
        
        public override Task<CreatePaymentCommandResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class CreatePaymentCommandResponse : BaseResponseModel
    {
    }
}