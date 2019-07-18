using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayCore.Architecture;
using PaymentGatewayCore.Model;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayCore.Query
{
    public class GetPaymentQuery : IRequest<GetPaymentQueryResult>
    {
        public Guid PaymentUid { get; }

        public GetPaymentQuery(Guid paymentUid)
        {
            PaymentUid = paymentUid;
        }
    }
    
    [UsedImplicitly]
    public class GetPaymentQueryHandler : BaseRequestHandler<GetPaymentQuery, GetPaymentQueryResult>
    {
        private readonly IQueryInterface<Payment> _paymentQueryInterface;

        public GetPaymentQueryHandler(IQueryInterface<Payment> paymentQueryInterface)
        {
            _paymentQueryInterface = paymentQueryInterface;
        }
        
        public override async Task<GetPaymentQueryResult> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var paymentResponse = await _paymentQueryInterface.Query(p => p.Uid == request.PaymentUid)
                .Select(p => new GetPaymentQueryResult
                {
                    Amount = p.Amount,
                    Currency = p.Currency,
                    Uid = p.Uid,
                    PaymentStatus = p.PaymentStatus,
                    CreatedDateUtc = p.CreatedDateUtc,
                    ObfuscatedCardNumber = p.ObfuscatedCardNumber
                }).SingleOrDefaultAsync(cancellationToken);
            
            if (paymentResponse == null)
                return Error(ErrorCode.NotFound, "We were not able to find the payment");
            
            return Success(paymentResponse);
        }
    }
    
    public class GetPaymentQueryResult : BaseResponseModel
    {
        public Guid Uid { get; set; }
        public string ObfuscatedCardNumber { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTimeOffset CreatedDateUtc { get; set; }
    }
}