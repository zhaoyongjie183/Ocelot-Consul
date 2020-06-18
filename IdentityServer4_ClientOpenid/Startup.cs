using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4_ClientOpenid
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";// CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";//OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "mvc client";
                    options.ClientSecret = "mvc secret";
                    options.SaveTokens = true;
                    options.ResponseType = "code";

                    options.Scope.Clear();
                    options.Scope.Add("api1");
                    options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                    options.Scope.Add(OidcConstants.StandardScopes.Profile);
                    options.Scope.Add(OidcConstants.StandardScopes.Email);
                    options.Scope.Add(OidcConstants.StandardScopes.Phone);
                    options.Scope.Add(OidcConstants.StandardScopes.Address);
                    options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);
                });

            //*====================这是implit模式=====================================
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "Cookies";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.SignInScheme = "Cookies";

            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;

            //        options.ClientId = "mvc";
            //        options.SaveTokens = true;
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
