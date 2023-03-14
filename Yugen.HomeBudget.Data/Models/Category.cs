namespace Yugen.HomeBudget.Data.Models;

public class Category : Entity
{
    public Category()
    {
    }

    public Category(
        string title,
        int? createdByApplicationUserId)
    {
        Title = title;
        SubCategories = new List<SubCategory>();
        CreatedByApplicationUserId = createdByApplicationUserId;
    }

    public string Title { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; }

    public ICollection<Expense> Expenses { get; set; }
}