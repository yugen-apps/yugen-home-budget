using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Data;
using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connection = string.Empty;
//if (builder.Environment.IsDevelopment())
//{
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
//}

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

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<ApplicationDbContext>();
//    context.Database.EnsureCreated();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();