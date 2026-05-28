using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Yugen.Home.Budget.Data;

namespace Yugen.Common.Blazor.Services.Info;

public class InfoService : IInfoService
{
	private const double Mebi = 1024 * 1024;
	private const double Gibi = Mebi * 1024;

	private readonly IConfiguration _configuration;
	private readonly ApplicationDbContext _dbContext;
	private readonly IWebHostEnvironment _env;

	private readonly string _hostName = Dns.GetHostName();
	private readonly EnvironmentInfo _envInfo = new();
	private IPAddress[] _ipList = [];
	private string _txt;
	private Dictionary<string, string> _list = [];

	public InfoService(
		IConfiguration configuration,
		ApplicationDbContext dbContext,
		IWebHostEnvironment env)
	{
		//_authService = authService;
		_configuration = configuration;
		_dbContext = dbContext;
		_env = env;
	}

	public async Task<Dictionary<string, string>> GetAsync(bool isAuthenticated)
	{
		_ipList = await Dns.GetHostAddressesAsync(_hostName);

		_list = new Dictionary<string, string>
		{
			{ "Version", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty },
			{ "Environment", _configuration?["ASPNETCORE_ENVIRONMENT"] ?? string.Empty },
			{ "CanConnect", _dbContext.Database.CanConnect().ToString() },
			{ "IsDevelopment", _env.IsDevelopment().ToString() },
			{ ".NET version", RuntimeInformation.FrameworkDescription },
			{ "Operating system", RuntimeInformation.OSDescription },
			{ "Processor architecture", RuntimeInformation.OSArchitecture.ToString() },
			{ "CPU cores", Environment.ProcessorCount.ToString() },
			{ "Containerized", Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is null ? "false" : "true" },
			{ "User", Environment.UserName },
			{ "cgroup memory limit", $"{_envInfo.MemoryLimit} {GetInBestUnit(_envInfo.MemoryLimit)}" },
			{ "cgroup memory usage", $"{_envInfo.MemoryUsage} {GetInBestUnit(_envInfo.MemoryUsage)}" },
			{ "Memory, total available GC memory", $"{_envInfo.TotalAvailableMemoryBytes} {GetInBestUnit(_envInfo.TotalAvailableMemoryBytes)}" },
			{ "Host name", _hostName },
			{ "Server IP address", string.Join(Environment.NewLine, _ipList) }
		};

		if (isAuthenticated)
		{
			_list.Add("Authentication:Google:ClientId", _configuration?["Authentication:Google:ClientId"] ?? string.Empty);
			_list.Add("Env Authentication:Google:ClientId", Environment.GetEnvironmentVariable("Authentication:Google:ClientId") ?? string.Empty);

			_list.Add("Authentication:Google:ClientSecret", _configuration?["Authentication:Google:ClientSecret"] ?? string.Empty);
			_list.Add("Env Authentication:Google:ClientSecret", Environment.GetEnvironmentVariable("Authentication:Google:ClientSecret") ?? string.Empty);

			_list.Add("ConfigurationConnectionString", _configuration?.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? string.Empty);
			_list.Add("EnvConnectionString", Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING") ?? string.Empty);
		}

		return _list;
	}

	public bool CanConnect() => _dbContext.Database.CanConnect();

	public void TestRead()
	{
		try
		{
			_txt = File.ReadAllText(System.IO.Path.Combine("/volume_dir", "test.txt"));
		}
		catch (Exception ex)
		{
			_txt = ex.Message;
		}

		_list.Add("Read test", _txt);
	}

	public void TestWrite()
	{
		try
		{
			File.WriteAllText(System.IO.Path.Combine("/volume_dir", "test.txt"), "Hello World");
		}
		catch (Exception ex)
		{
			_txt = ex.Message;
		}

		_list.Add("Write test", _txt);
	}

	private static string GetInBestUnit(long size)
	{
		if (size < Mebi)
		{
			return $"{size} bytes";
		}
		else if (size < Gibi)
		{
			double mebibytes = size / Mebi;
			return $"{mebibytes:N2} MiB";
		}
		else
		{
			double gibibytes = size / Gibi;
			return $"{gibibytes:N2} GiB";
		}
	}
}