using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PaymentGatewayCore.Architecture;
using PaymentGatewayCore.Model;

namespace PaymentGatewayCore.Command
{
    /// <summary>
    /// Command to create a payment request
    /// </summary>
    public class CreatePaymentCommand : IRequest<CreatePaymentCommandResponse>
    {
        
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