using System;
using AutoMapper;
using JetBrains.Annotations;
using PaymentGatewayCore.Query;

namespace PaymentGatewayAPI.Model
{
    public class DetailedPaymentResponse : BasicPaymentResponse
    {
        /// <summary>
        /// Card number that has been obfuscated to protect the customer
        /// </summary>
        public string CardNumber { get; set; }
        
        /// <summary>
        /// The amount that was charged
        /// </summary>
        public int Amount { get; set; }
        
        /// <summary>
        /// The currency of the charge
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// When this transaction was carried out
        /// </summary>
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