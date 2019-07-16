using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGatewayDatabase.Models;

namespace PaymentGatewayDatabase
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Payment>(ConfigurePayment);
        }

        private void ConfigurePayment(EntityTypeBuilder<Payment> entity)
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Uid)
                .IsRequired();

            entity.Property(e => e.Amount)
                .IsRequired();
            
            entity.Property(e => e.Currency)
                .IsRequired();
            
            entity.Property(e => e.ObfuscatedCardNumber)
                .IsRequired();
            
            entity.Property(e => e.PaymentStatus)
                .IsRequired();
            
            entity.Property(e => e.CreatedDateUtc)
                .IsRequired();
        }
    }
}