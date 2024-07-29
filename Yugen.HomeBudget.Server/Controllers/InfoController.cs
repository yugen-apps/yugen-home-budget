using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Yugen.HomeBudget.Data;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Server.Controllers;

[ApiController]
[Route($"{EndpointConstants.Prefix}/[controller]/[action]")]
public class InfoController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _env;

    public InfoController(
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _env = env;
    }

    [HttpGet]
    public Dictionary<string, string> Get()
    {
        var List = new Dictionary<string, string>
		{
			{ "Version", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? string.Empty },
			{ "Environment", _configuration?["ASPNETCORE_ENVIRONMENT"] ?? string.Empty },
			{ "ConfigurationConnectionString", _configuration?.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? string.Empty }, // ApplicationDbContext
			{ "EnvConnectionString", Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING") ?? string.Empty },
			{ "CanConnect", _dbContext.Database.CanConnect().ToString() },
			{ "IsDevelopment", _env.IsDevelopment().ToString() }
		};
        return List;
    }
}