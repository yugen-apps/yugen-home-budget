namespace Yugen.HomeBudget.Data.Models;

public class Category : Entity
{
    public Category()
    {
    }

    public Category(
        string title,
        string icon,
        int? createdByApplicationUserId)
    {
        Title = title;
        Icon = icon;
        SubCategories = [];
        CreatedByApplicationUserId = createdByApplicationUserId;
    }

    public string Title { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public ICollection<SubCategory> SubCategories { get; set; } = [];

    public ICollection<Expense> Expenses { get; set; } = [];
}