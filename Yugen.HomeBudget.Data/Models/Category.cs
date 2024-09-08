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
        CreatedByApplicationUserId = createdByApplicationUserId;
    }

    public string? Title { get; set; }

    public string? Icon { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; } = [];

    public ICollection<Expense> Expenses { get; set; } = [];
}