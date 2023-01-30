using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Yugen.HomeBudget.Client;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Client.ViewModels;
using Yugen.HomeBudget.Client.ViewModels.Authentication;
using Yugen.HomeBudget.Client.ViewModels.Category;
using Yugen.HomeBudget.Client.ViewModels.Expense;
using Yugen.HomeBudget.Client.ViewModels.Info;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient("Yugen.HomeBudget.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Yugen.HomeBudget.ServerAPI"));

builder.Services.AddScoped<IndexViewModel>();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<RegisterViewModel>();
builder.Services.AddScoped<CategoryAddEditViewModel>();
builder.Services.AddScoped<CategoryListViewModel>();
builder.Services.AddScoped<ExpenseAddEditViewModel>();
builder.Services.AddScoped<ExpenseListViewModel>();
builder.Services.AddScoped<InfoIndexViewodel>();

await builder.Build().RunAsync();