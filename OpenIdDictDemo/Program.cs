using AuthorizationServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace OpenIdDictDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services=builder.Services;



            //Services
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                         options.LoginPath = "/account/login";

                    });
            services.AddHostedService<TestData>();
            services.AddDbContext<DbContext>(options =>
            {
                // Configure the context to use an in-memory store.
                options.UseInMemoryDatabase(nameof(DbContext));

                // Register the entity sets needed by OpenIddict.
                options.UseOpenIddict();

            });

            services.AddOpenIddict()

                // Register the OpenIddict core components.
            .AddCore(options =>
            {
            // Configure OpenIddict to use the EF Core stores/models.
                 options.UseEntityFrameworkCore()
                .UseDbContext<DbContext>();
             })

                // Register the OpenIddict server components.
            .AddServer(options =>
            {
                 options
                        .AllowClientCredentialsFlow();

                 options
                     .SetTokenEndpointUris("/connect/token");

                 // Encryption and signing of tokens
                options
                     .AddEphemeralEncryptionKey()
                     .AddEphemeralSigningKey();

                // Register scopes (permissions)
                 options.RegisterScopes("api");

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                 options
                     .UseAspNetCore()
                     .EnableTokenEndpointPassthrough();
                options
                     .AddEphemeralEncryptionKey()
                     .AddEphemeralSigningKey()
                     .DisableAccessTokenEncryption();
            });





            var app = builder.Build();

            //Middlewares

            

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            app.Run();
        }
    }
}