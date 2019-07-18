using System.Collections.Generic;
using AutoMapper;
using JetBrains.Annotations;
using PaymentGatewayCore.Query;

namespace PaymentGatewayAPI.Model
{
    public class GetListOfPaymentsResponse
    {
        public List<BasicPaymentResponse> Payments { get; set; }
    }
    
    [UsedImplicitly]
    public class GetListOfPaymentsResponseProfile : Profile
    {
        public GetListOfPaymentsResponseProfile()
        {
            CreateMap<GetPaymentsQueryResponse, GetListOfPaymentsResponse>()
                .ForMember(dest => dest.Payments, source => source.MapFrom(s => s.PaymentResults));
        }
    }
}