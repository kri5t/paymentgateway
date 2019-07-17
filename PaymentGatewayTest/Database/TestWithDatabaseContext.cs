using System;
using PaymentGatewayDatabase;

namespace PaymentGatewayTest.Database
{
    public class TestWithDatabaseContext : IDisposable
    {
        protected DatabaseContext DatabaseContext;

        public TestWithDatabaseContext()
        {
            DatabaseContext = new InMemoryRecipeDbContext().GetRecipeDbContext();
        }

        public void Dispose()
        {
            DatabaseContext.Database.EnsureDeleted();
            DatabaseContext.Dispose();
        }
    }
}