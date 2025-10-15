namespace Yugen.HomeBudget.Server.Components.Layout
{
    public partial class TopMenu
    {
        private static bool IsAccount => Routes.CurrentUrl?.StartsWith("Account") ?? false;
    }
}