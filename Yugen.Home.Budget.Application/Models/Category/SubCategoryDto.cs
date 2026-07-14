namespace Yugen.Home.Budget.Application.Models.Category;

public class SubCategoryDto
{
    public SubCategoryDto(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public SubCategoryDto()
    {
        Id = 0;
        Title = string.Empty;
    }

    public int Id { get; set; }

    public string Title { get; set; }
}