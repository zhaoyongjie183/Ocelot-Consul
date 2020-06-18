using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace OcelotIdentityService
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
            services.AddOcelot().AddConsul().AddPolly();
            services.AddAuthentication()
                .AddIdentityServerAuthentication("client_b", options =>
                {
                    options.Authority = "http://localhost:5823";
                    options.ApiName = "Api_ServerB";
                    options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
                    options.ApiSecret = "secret";
                    options.RequireHttpsMetadata = false;

                }).AddIdentityServerAuthentication("client_a", options =>
                {
                    options.Authority = "http://localhost:5823";
                    options.ApiName = "Api_ServerA";
                    options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Both;
                    options.ApiSecret = "secret";
                    options.RequireHttpsMetadata = false;

                }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot();
          
          
        }
    }
}
