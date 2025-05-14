namespace Yugen.HomeBudget.Client.Helpers;

public static class IconHelper
{
    public static IconName GetIconName(string icon)
    {
        if (string.IsNullOrWhiteSpace(icon))
        {
            return IconName.Bold;
        }

        return Enum.TryParse(icon, true, out IconName iconName) ? iconName : IconName.Bold;
    }
}