using System.Reflection;

namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class FooterMenu
    {
        //private readonly string? _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        private readonly string? _version = $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version}";
    }
}