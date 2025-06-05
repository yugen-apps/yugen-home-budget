using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Client.ViewModels;
using Yugen.HomeBudget.Client.ViewModels.Authentication;
using Yugen.HomeBudget.Client.ViewModels.Category;
using Yugen.HomeBudget.Client.ViewModels.Expense;
using Yugen.HomeBudget.Client.ViewModels.Info;
using Yugen.HomeBudget.Data;
using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Data.Repositories;
using Yugen.HomeBudget.Server.AuthenticationProviders;
using Yugen.HomeBudget.Server.Components;

var builder = WebApplication.CreateBuilder(args);
var baseAddress = builder.Configuration.GetValue<string>("BaseUrl");

var connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");

var azureSqlTokenProvider = new AzureCredentialSqlAuthenticationProvider(builder.Environment.IsDevelopment());
SqlAuthenticationProvider.SetProvider(SqlAuthenticationMethod.ActiveDirectoryManagedIdentity, azureSqlTokenProvider);
SqlAuthenticationProvider.SetProvider(SqlAuthenticationMethod.ActiveDirectoryMSI, azureSqlTokenProvider);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection,
                                                    b => b.MigrationsAssembly("Yugen.HomeBudget.Data")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

// Add services to the container.
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ExpenseRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ExpenseService>();

builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient("Yugen.HomeBudget.ServerAPI", client => client.BaseAddress = new Uri("https://localhost:7288/"));
// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                                   .CreateClient("Yugen.HomeBudget.ServerAPI"));

builder.Services.AddScoped<IndexViewModel>();
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<RegisterViewModel>();
builder.Services.AddScoped<CategoryAddEditViewModel>();
builder.Services.AddScoped<CategoryListViewModel>();
builder.Services.AddScoped<ExpenseAddEditViewModel>();
builder.Services.AddScoped<ExpenseListViewModel>();
builder.Services.AddScoped<InfoIndexViewodel>();

builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

AddBlazorise(builder.Services);

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<ApplicationDbContext>();
//    context.Database.EnsureCreated();
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Yugen.HomeBudget.Client.Components._Imports).Assembly);

app.Run();

static void AddBlazorise(IServiceCollection services)
{
    services
        .AddBlazorise();
    services
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons();
}