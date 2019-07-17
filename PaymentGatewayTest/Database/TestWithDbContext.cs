using System;
using PaymentGatewayDatabase;

namespace PaymentGatewayTest.Database
{
    public class TestWithDbContext : IDisposable
    {
        protected DatabaseContext DbContext;

        public TestWithDbContext()
        {
            DbContext = new InMemoryRecipeDbContext().GetRecipeDbContext();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }
}