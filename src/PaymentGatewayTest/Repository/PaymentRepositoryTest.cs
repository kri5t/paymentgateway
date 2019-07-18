using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentGatewayCore.Repository;
using PaymentGatewayDatabase.Models;
using PaymentGatewayTest.Database;
using Xunit;

namespace PaymentGatewayTest.Repository
{
    public class PaymentRepositoryTest : TestWithDatabaseContext
    {
        private readonly Guid _bankTransactionUid;
        private readonly DateTimeOffset _createdDateUtc;
        private const string CardNumber = "1234123412341234";
        private const string ObfuscatedCardNumber = "xxxxxxxxxxxx1234";
        private const string Currency = "DKK";
        private const int Amount = 30;

        public PaymentRepositoryTest()
        {
            _bankTransactionUid = Guid.NewGuid();
            _createdDateUtc = DateTimeOffset.UtcNow;
        }
        
        [Fact]
        public async Task CreatePaymentAsync_InsertsPaymentInDB(){
            //Setup
            var paymentRepository = new PaymentRepository(DatabaseContext);
            var paymentStatus = PaymentStatus.PaymentSucceeded;
            //Test
            await paymentRepository.CreatePaymentAsync(CancellationToken.None, CardNumber, Amount, Currency, 
                paymentStatus, _bankTransactionUid, _createdDateUtc);
            await paymentRepository.SaveAsync(CancellationToken.None);
            
            //Verify
            Assert.Collection(DatabaseContext.Payments, payment =>
            {
                Assert.Equal(ObfuscatedCardNumber, payment.ObfuscatedCardNumber);
                Assert.Equal(Amount, payment.Amount);
                Assert.Equal(Currency, payment.Currency);
                Assert.Equal(1, payment.Id);
                Assert.Equal(paymentStatus, payment.PaymentStatus);
                Assert.Equal(_createdDateUtc, payment.CreatedDateUtc);
                Assert.Equal(_bankTransactionUid, payment.BankTransactionUid);
                Assert.NotEqual(default(Guid), payment.Uid);
            });
        }
    }
}