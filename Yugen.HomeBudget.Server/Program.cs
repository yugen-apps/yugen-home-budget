using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using System;
using System.Net.Http;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Data;
using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Data.Repositories;
using Yugen.HomeBudget.Server.Components.Account;
using Yugen.HomeBudget.Server.Models.Account;
using Yugen.HomeBudget.Server.Services;
using Yugen.HomeBudget.Server.ViewModels;
using Yugen.HomeBudget.Server.ViewModels.Category;
using Yugen.HomeBudget.Server.ViewModels.Expense;
using Yugen.HomeBudget.Server.ViewModels.Info;

namespace Yugen.HomeBudget.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents()
                .AddAuthenticationStateSerialization();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId") ?? string.Empty;
                    options.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret") ?? string.Empty;
                    options.ClaimActions.MapJsonKey(GoogleClaimTypes.Picture, "picture");
                })
                .AddIdentityCookies();

#if DEBUG
            AddDbContext(builder, false);
#else
            AddDbContext(builder, false);
#endif

            builder.Services.AddIdentityCore<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<ExpenseRepository>();

            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ExpenseService>();
            builder.Services.AddScoped<InfoService>();

            builder.Services.AddScoped<AuthService>();

            var baseAddress = builder.Configuration.GetValue<string>("BaseUrl") ?? string.Empty;
            builder.Services.AddHttpClient("Yugen.HomeBudget.ServerAPI", client => client.BaseAddress = new Uri(baseAddress));
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                                               .CreateClient("Yugen.HomeBudget.ServerAPI"));

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<CategoryDetailsViewModel>();
            builder.Services.AddTransient<CategoryListViewModel>();
            builder.Services.AddTransient<ExpenseDetailsViewModel>();
            builder.Services.AddTransient<ExpenseListViewModel>();
            builder.Services.AddTransient<InfoIndexViewodel>();

            builder.Services.AddMudServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<Components.App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            PingDb(app);

            app.Run();
        }

        private static void AddDbContext(WebApplicationBuilder builder, bool useInMemoryDatabase)
        {
            var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"); // ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            if (useInMemoryDatabase)
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("db"));
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
                                                                                                    b => b.MigrationsAssembly("Yugen.HomeBudget.Data")),
                                                                    ServiceLifetime.Transient);
            }
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }

        private static void PingDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var infoService = scope.ServiceProvider.GetRequiredService<InfoService>();
            infoService.CanConnect();
        }
    }
}
