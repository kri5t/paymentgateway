using System;
using PaymentGatewayCore.Aggregate;
using PaymentGatewayDatabase.Models;
using Xunit;

namespace PaymentGatewayTest.Aggregate
{
    public class PaymentAggregateTest
    {
        private readonly Guid _bankTransactionUid;
        private readonly DateTimeOffset _createdDateUtc;
        private const string CardNumber = "1234123412341234";
        private const string TransformedCardNumber = "xxxxxxxxxxxx1234";
        private const string Currency = "DKK";
        private const int Amount = 30;
        
        
        public PaymentAggregateTest()
        {
            _bankTransactionUid = Guid.NewGuid();
            _createdDateUtc = DateTimeOffset.UtcNow;
        }
        
        [Fact]
        public void ReturnsAggregate_WithCorrectParameters(){
            //Setup
               
            //Test
            var sut = new PaymentAggregate(CardNumber, Amount, Currency, PaymentStatus.PaymentSucceeded, _bankTransactionUid, _createdDateUtc);
            
            //Verify
            Assert.Equal(TransformedCardNumber, sut.State.ObfuscatedCardNumber);
            Assert.Equal(Amount, sut.State.Amount);
            Assert.Equal(Currency, sut.State.Currency);
            Assert.Equal(_bankTransactionUid, sut.State.BankTransactionUid);
            Assert.Equal(_createdDateUtc, sut.State.CreatedDateUtc);
            Assert.NotEqual(default(Guid), sut.State.Uid);
        }
        
        [Fact]
        public void ThrowsException_OnWrongCreditCard(){
            //Setup
               
            //Test
            
            //Verify
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentAggregate("FAILED_CARD", Amount, Currency, PaymentStatus.PaymentSucceeded, _bankTransactionUid, _createdDateUtc));
        }
        
        [Fact]
        public void ThrowsException_OnEmptyCreditCard(){
            //Setup
               
            //Test
            
            //Verify
            Assert.Throws<ArgumentNullException>(() => new PaymentAggregate(string.Empty, Amount, Currency, PaymentStatus.PaymentSucceeded, _bankTransactionUid, _createdDateUtc));
        }
        
        [Fact]
        public void ThrowsException_OnNegativeAmount(){
            //Setup
               
            //Test
            
            //Verify
            Assert.Throws<ArgumentOutOfRangeException>(() => new PaymentAggregate(CardNumber, -1, Currency, PaymentStatus.PaymentSucceeded, _bankTransactionUid, _createdDateUtc));
        }
        
        [Fact]
        public void ThrowsException_OnEmptyCurrency(){
            //Setup
               
            //Test
            
            //Verify
            Assert.Throws<ArgumentNullException>(() => new PaymentAggregate(CardNumber, Amount, string.Empty, PaymentStatus.PaymentSucceeded, _bankTransactionUid, _createdDateUtc));
        }
        
        [Fact]
        public void ThrowsException_OnDefaultTransactionUid(){
            //Setup
               
            //Test
            
            //Verify
            Assert.Throws<ArgumentNullException>(() => new PaymentAggregate(CardNumber, Amount, string.Empty, PaymentStatus.PaymentSucceeded, default(Guid), _createdDateUtc));
        }
    }
}