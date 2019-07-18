using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using PaymentGatewayCore;
using PaymentGatewayDatabase;
using Swashbuckle.AspNetCore.Swagger;

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
            Mapper.Initialize(config => config.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            Mapper.Configuration.AssertConfigurationIsValid();
            //END AutoMapper configs

            var connectionString = Configuration["ConnectionString"];
            
            services
                .AddCoreServices(connectionString)
                .AddMvc();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Payment API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
//            if (env.IsDevelopment())
//                app.UseDeveloperExceptionPage();

            UpdateDatabase(app);
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API");
            });

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