using System;
using AutoMapper;
using JetBrains.Annotations;
using PaymentGatewayCore.Query;

namespace PaymentGatewayAPI.Model
{
    public class DetailedPaymentResponse : BasicPaymentResponse
    {
        public string CardNumber { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset CreatedDateUtc { get; set; }
    }

    [UsedImplicitly]
    public class DetailedPaymentResponseProfile : Profile
    {
        public DetailedPaymentResponseProfile()
        {
            CreateMap<GetPaymentQueryResult, DetailedPaymentResponse>()
                .ForMember(dest => dest.Uid, source => source.MapFrom(s => s.Uid))
                .ForMember(dest => dest.PaymentStatus, source => source.MapFrom(s => s.PaymentStatus.ToString()))
                .ForMember(dest => dest.Amount, source => source.MapFrom(s => s.Amount))
                .ForMember(dest => dest.Currency, source => source.MapFrom(s => s.Currency))
                .ForMember(dest => dest.CreatedDateUtc, source => source.MapFrom(s => s.CreatedDateUtc))
                .ForMember(dest => dest.CardNumber, source => source.MapFrom(s => s.ObfuscatedCardNumber))
                ;
        }
    }
}