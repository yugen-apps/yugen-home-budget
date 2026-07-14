using System.Reflection;

namespace Yugen.Home.Budget.Server.Components.Layout;

public partial class SideMenu
{
    //private readonly string? _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
    private readonly string _version = $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version}";
}