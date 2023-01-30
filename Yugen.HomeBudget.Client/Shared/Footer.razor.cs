using System.Reflection;

namespace Yugen.HomeBudget.Client.Shared
{
    public partial class Footer
    {
        private string? _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}