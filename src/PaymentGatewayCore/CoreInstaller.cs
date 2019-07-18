using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentGatewayCore.Architecture;
using PaymentGatewayCore.BankMock;
using PaymentGatewayCore.Repository;
using PaymentGatewayDatabase;

namespace PaymentGatewayCore
{
    public static class CoreInstaller
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<DatabaseContext>(o =>
                    o.UseSqlServer(connectionString), ServiceLifetime.Transient)
                .BuildServiceProvider();
            services.AddMediatR(typeof(CoreInstaller).Assembly);
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IBank, Bank>();
            services.AddTransient(typeof(IQueryInterface<>), typeof(QueryInterface<>));
            return services;
        }
    }
}