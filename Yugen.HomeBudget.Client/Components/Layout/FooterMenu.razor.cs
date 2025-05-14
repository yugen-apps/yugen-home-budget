using System.Reflection;

namespace Yugen.HomeBudget.Client.Components.Layout
{
    public partial class FooterMenu
    {
        private readonly string? _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
    }
}