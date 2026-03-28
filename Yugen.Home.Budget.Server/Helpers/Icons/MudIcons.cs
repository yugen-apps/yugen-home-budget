namespace Yugen.Home.Budget.Server.Helpers.Icons;

public class MudIcons
{
    public string Name { get; }

    public string Code { get; }

    public string Category { get; }

    public MudIcons(string name, string code, string category)
    {
        Name = name;
        Code = code;
        Category = category;
    }

    public static readonly MudIcons Empty = new(string.Empty, string.Empty, string.Empty);
}