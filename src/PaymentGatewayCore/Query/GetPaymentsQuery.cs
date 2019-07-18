using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayCore.Architecture;
using PaymentGatewayCore.Model;
using PaymentGatewayDatabase;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayCore.Query
{
    public class GetPaymentsQuery : IRequest<GetPaymentsQueryResponse>
    {}
    
    [UsedImplicitly]
    public class GetPaymentsQueryHandler : BaseRequestHandler<GetPaymentsQuery, GetPaymentsQueryResponse>
    {
        private readonly IQueryInterface<Payment> _paymentQueryInterface;

        public GetPaymentsQueryHandler(IQueryInterface<Payment> paymentQueryInterface)
        {
            _paymentQueryInterface = paymentQueryInterface;
        }
        
        public override async Task<GetPaymentsQueryResponse> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentQueryInterface.Query(p => true)
                .Select(payment => new BasicPaymentResult
                {
                    Uid = payment.Uid,
                    PaymentStatus = payment.PaymentStatus
                }).ToListAsync(cancellationToken);

            return Success(new GetPaymentsQueryResponse
            {
                PaymentResults = payments
            });
        }
    }
    
    public class GetPaymentsQueryResponse : BaseResponseModel
    {
        public List<BasicPaymentResult> PaymentResults { get; set; }
    }

    public class BasicPaymentResult
    {
        public Guid Uid { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}