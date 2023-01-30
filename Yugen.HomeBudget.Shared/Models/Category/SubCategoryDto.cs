namespace Yugen.HomeBudget.Shared.Models.Category;

public class SubCategoryDto
{
    public SubCategoryDto(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; set; }

    public string Title { get; set; }
}