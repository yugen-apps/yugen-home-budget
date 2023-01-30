namespace Yugen.HomeBudget.Shared.Models.Category;

public class CreateCategoryDto
{
    public CreateCategoryDto(
        string title,
        List<SubCategoryDto> subCategoriesDto)
    {
        Title = title;
        SubCategoriesDto = subCategoriesDto;
    }

    public string Title { get; set; }

    public List<SubCategoryDto> SubCategoriesDto { get; set; }
}