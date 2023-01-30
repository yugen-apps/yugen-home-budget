using System.Reflection;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class Footer
    {
        private readonly string? _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
    }
}