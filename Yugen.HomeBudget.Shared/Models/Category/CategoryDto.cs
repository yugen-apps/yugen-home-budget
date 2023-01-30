namespace Yugen.HomeBudget.Shared.Models.Category;

public class CategoryDto
{
    public CategoryDto(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public List<SubCategoryDto> SubCategoriesDto { get; set; } = new List<SubCategoryDto>();
}