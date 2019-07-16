using System;

namespace PaymentGatewayDatabase.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string ObfuscatedCardNumber { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Guid BankTransactionUid { get; set; }
        public DateTimeOffset CreatedDateUtc { get; set; }
    }

    public enum PaymentStatus : int
    {
        PaymentDeclined = 0,
        PaymentFailed = 1,
        PaymentSucceeded = 2
    }
}