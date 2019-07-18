using System;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayDatabase;

namespace PaymentGatewayTest.Database
{
    public class InMemoryDatabaseContext
    {
        public DatabaseContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DatabaseContext(options);
        }
    }
}