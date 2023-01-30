namespace Yugen.HomeBudget.Data.Models;

public class Category
{
    public Category()
    {
    }

    public Category(string title)
    {
        Title = title;
        SubCategories = new List<SubCategory>();
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; }

    public ICollection<Expense> Expenses { get; set; }
}