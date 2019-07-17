using System;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayDatabase;

namespace PaymentGatewayTest.Database
{
    public class InMemoryRecipeDbContext
    {
        public DatabaseContext GetRecipeDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DatabaseContext(options);
        }
    }
}