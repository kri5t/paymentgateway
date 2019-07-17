using System;
using System.Threading;
using System.Threading.Tasks;
using PaymentGatewayCore.Aggregate;
using PaymentGatewayDatabase;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayCore.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DatabaseContext _database;

        public PaymentRepository(DatabaseContext database)
        {
            _database = database;
        }
        
        public async Task<PaymentAggregate> CreatePaymentAsync(CancellationToken cancellationToken, string cardNumber, 
            int amount, string currency, PaymentStatus paymentStatus, Guid bankTransactionUid, 
            DateTimeOffset createdDateUtc)
        {
            var aggregate = new PaymentAggregate(cardNumber, amount, currency, paymentStatus, bankTransactionUid, createdDateUtc);
            await _database.Payments.AddAsync(aggregate.State, cancellationToken);
            return aggregate;
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _database.SaveChangesAsync(cancellationToken);
        }
    }

    public interface IPaymentRepository
    {
        Task<PaymentAggregate> CreatePaymentAsync(CancellationToken cancellationToken, string cardNumber, 
            int amount, string currency, PaymentStatus paymentStatus, Guid bankTransactionUid, 
            DateTimeOffset createdDateUtc);

        Task SaveAsync(CancellationToken cancellationToken);
    }
}