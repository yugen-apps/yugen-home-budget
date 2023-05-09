using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Models;

public class Category
{
    public Category()
    {
    }

    public Category(
        int id,
        string title)
    {
        Id = id;
        Title = title;
    }

    public Category(
        int id,
        string title,
        List<SubCategoryDto> subCategoriesDto) : this(id, title)
    {
        SubCategoriesDto = subCategoriesDto;
    }

    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<SubCategoryDto> SubCategoriesDto { get; set; } = new List<SubCategoryDto>();
}