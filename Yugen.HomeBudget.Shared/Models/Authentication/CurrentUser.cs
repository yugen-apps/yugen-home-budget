namespace Yugen.HomeBudget.Shared.Models.Authentication;

public class CurrentUser
{
    public bool IsAuthenticated { get; set; }

    public string UserName { get; set; } = string.Empty;

    public Dictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

    public int? Id { get; set; }
}