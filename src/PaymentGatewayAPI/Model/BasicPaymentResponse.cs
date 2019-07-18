using System;
using AutoMapper;
using JetBrains.Annotations;
using PaymentGatewayCore.Query;

namespace PaymentGatewayAPI.Model
{
    public class BasicPaymentResponse
    {
        public Guid Uid { get; set; }
        public string PaymentStatus { get; set; }
    }

    [UsedImplicitly]
    public class BasicPaymentResponseProfile : Profile
    {
        public BasicPaymentResponseProfile()
        {
            CreateMap<BasicPaymentResult, BasicPaymentResponse>()
                .ForMember(dest => dest.Uid, source => source.MapFrom(s => s.Uid))
                .ForMember(dest => dest.PaymentStatus, source => source.MapFrom(s => s.PaymentStatus.ToString()));
        }
    }
}