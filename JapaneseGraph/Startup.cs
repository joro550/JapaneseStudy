using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JapaneseGraph.Shared;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JapaneseGraph
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration) 
            => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.Configure<FirebaseConfig>(
                _configuration.GetSection(nameof(FirebaseConfig)));
            
            services
                .AddGraphQLServer()
                .AddQueryType<Query>();

            services.AddTransient<FirebaseFactory>();
            services.AddMediatR(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            
            app.UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
        }
    }
}