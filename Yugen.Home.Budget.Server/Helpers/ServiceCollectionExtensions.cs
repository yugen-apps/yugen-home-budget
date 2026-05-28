using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account;
using Yugen.Common.Blazor.Account.Services;
using Yugen.Common.Blazor.Components.LoadingSpinner;
using Yugen.Common.Blazor.Services.Info;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Data;
using Yugen.Home.Budget.Data.Models;
using Yugen.Home.Budget.Data.Repositories;
using Yugen.Home.Budget.Server.ViewModels;
using Yugen.Home.Budget.Server.ViewModels.Category;
using Yugen.Home.Budget.Server.ViewModels.Expense;
using Yugen.Home.Budget.Server.ViewModels.Info;

namespace Yugen.Home.Budget.Server.Helpers;

public static class ServiceCollectionExtensions
{
	private const string ServerHttpClient = "ServerHttpClient";
	private const string DataProject = "Yugen.Home.Budget.Data";

	public static void ConfigureServices(
		this IServiceCollection services,
		ConfigurationManager configuration)
	{
		var useInMemoryDatabase = configuration.GetValue<bool>("UseInMemoryDatabase");
		if (useInMemoryDatabase)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("db"));
		}
		else
		{
			var connectionString = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
				// ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
																						b => b.MigrationsAssembly(DataProject)),
																   ServiceLifetime.Transient);
		}

		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
		services.AddScoped<CategoryRepository>();
		services.AddScoped<ExpenseRepository>();

		services.AddScoped<CategoryService>();
		services.AddScoped<ExpenseService>();

		services.AddScoped<AuthService>();

		var baseAddress = configuration.GetValue<string>("BaseUrl") ?? string.Empty;
		services.AddHttpClient(ServerHttpClient, client => client.BaseAddress = new Uri(baseAddress));
		// Supply HttpClient instances that include access tokens when making requests to the server project
		services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
										   .CreateClient(ServerHttpClient));

		services.AddTransient<HomeViewModel>();
		services.AddTransient<CategoryDetailsViewModel>();
		services.AddTransient<CategoryListViewModel>();
		services.AddTransient<ExpenseDetailsViewModel>();
		services.AddTransient<ExpenseListViewModel>();
		services.AddTransient<InfoIndexViewodel>();

		services.AddScoped<ILoadingSpinnerService, LoadingSpinnerService>();
		services.AddScoped<IInfoService, InfoService>();
	}

	public static async Task InitializeServicesAsync(this IServiceProvider services)
	{
		using var scope = services.CreateScope();

		// Wake Up DB
		var infoService = scope.ServiceProvider.GetRequiredService<IInfoService>();
		while (!infoService.CanConnect())
		{
			await Task.Delay(10000);
		}

		using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		await context.Database.EnsureCreatedAsync();
	}
}
