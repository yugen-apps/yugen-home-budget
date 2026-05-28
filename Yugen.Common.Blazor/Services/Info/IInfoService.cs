using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yugen.Common.Blazor.Services.Info;

public interface IInfoService
{
	bool CanConnect();

	Task<Dictionary<string, string>> GetAsync(bool isAuthenticated);
	void TestRead();
	void TestWrite();
}

