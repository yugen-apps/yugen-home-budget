using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Yugen.HomeBudget.Data;

namespace Yugen.HomeBudget.Server.Services;

public class InfoService
{
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _env;

    public InfoService(
        AuthService authService,
        IConfiguration configuration,
        ApplicationDbContext dbContext,
        IWebHostEnvironment env)
    {
        _authService = authService;
        _configuration = configuration;
        _dbContext = dbContext;
        _env = env;
    }

    public async Task<Dictionary<string, string>> GetAsync()
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        
        var List = new Dictionary<string, string>
        {
            { "Version", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty },
            { "Environment", _configuration?["ASPNETCORE_ENVIRONMENT"] ?? string.Empty },
            { "ConfigurationConnectionString", currentUser.IsAuthenticated ? _configuration?.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? string.Empty : string.Empty},
            { "EnvConnectionString", currentUser.IsAuthenticated ? Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING") ?? string.Empty : string.Empty},
            { "CanConnect", _dbContext.Database.CanConnect().ToString() },
            { "IsDevelopment", _env.IsDevelopment().ToString() }
        };
        return List;
    }

    public bool CanConnect() => _dbContext.Database.CanConnect();
}