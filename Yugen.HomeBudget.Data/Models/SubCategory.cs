namespace Yugen.HomeBudget.Data.Models;

public class SubCategory
{
    public SubCategory()
    {
    }

    public SubCategory(string title)
    {
        Title = title;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }
}