namespace Yugen.HomeBudget.Shared.Models.Category;

public class UpdateCategoryDto
{
    public UpdateCategoryDto(
        int id,
        string title,
        List<SubCategoryDto> subCategoriesDto)
    {
        Id = id;
        Title = title;
        SubCategoriesDto = subCategoriesDto;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public List<SubCategoryDto> SubCategoriesDto { get; set; }
}