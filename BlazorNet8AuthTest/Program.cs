using BlazorNet8AuthTest.Components;
using Microsoft.AspNetCore.Authorization;

namespace BlazorNet8AuthTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddControllers();

            builder.Services
                .AddAuthentication("USER")
                .AddCookie("USER", options =>
                {
                    options.Cookie.Name = "USER";
                    options.SlidingExpiration = true;
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
                });

            builder.Services
                .AddAuthorization(options =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder("USER")
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("USER", authorizationPolicy);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app
                .UseRouting()
                .UseStaticFiles()
                .UseAuthentication()
                .UseAuthorization()
                .UseAntiforgery()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            //.AddHubOptions(); //This Extension is not available in Net8-RC2

            //Note: If i disable this MapBlazorHub Options i am not getting
            //any errors, While having this code i am getting Initializer problem
            app.MapBlazorHub(config =>
            {
                config.CloseOnAuthenticationExpiration = true;
            });

            app.Run();
        }
    }
}