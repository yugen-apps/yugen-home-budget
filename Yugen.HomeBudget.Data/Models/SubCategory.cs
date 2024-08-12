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
    
    public string Title { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}