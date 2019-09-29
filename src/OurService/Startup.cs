using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OurService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace OurService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddHttpClient<WeatherServiceWithRetry>( client =>
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
            })
            .AddPolicyHandler(ServicePolicies.GetCircuitBreakerPolicy())
            .AddPolicyHandler(ServicePolicies.GetRetryPolicy())
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));


            services.AddHttpClient<WeatherService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
            })
            .AddPolicyHandler(ServicePolicies.GetCircuitBreakerPolicy())
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
