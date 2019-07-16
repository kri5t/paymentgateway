using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using PaymentGatewayCore;
using PaymentGatewayDatabase;

namespace PaymentGatewayAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISystemClock, SystemClock>();
            //AutoMapper configs
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            Mapper.Initialize(x => x.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            Mapper.Configuration.AssertConfigurationIsValid();
            //END AutoMapper configs

            services
                .AddCoreServices(Configuration["ConnectionString"])
                .AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            UpdateDatabase(app);
            app.UseMvc();
        }
        
        /// <summary>
        /// Only used as a part of showcase.
        ///
        /// Normally the database migrations will be run as a part of the CI/CD pipe
        /// and not as part of the service startup 
        /// </summary>
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = 
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DatabaseContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}