using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentGatewayDatabase
{
    public class MigrationsDBContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();

            builder.UseSqlServer(
                "Server=127.0.0.1,1433;Database=PaymentGateway;User=sa;Password=aJyS68KNZP9;",
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(DatabaseContext)
                    .GetTypeInfo().Assembly.GetName().Name));

            return new DatabaseContext(builder.Options);
        }
    }
}