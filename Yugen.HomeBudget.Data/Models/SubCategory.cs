namespace Yugen.HomeBudget.Data.Models;

public class SubCategory : Entity
{
    public SubCategory()
    {
    }

    public SubCategory(string title)
    {
        Title = title;
    }

    public string? Title { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}