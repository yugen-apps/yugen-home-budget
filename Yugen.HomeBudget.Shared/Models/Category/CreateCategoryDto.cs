namespace Yugen.HomeBudget.Shared.Models.Category;

public class CreateCategoryDto
{
    public CreateCategoryDto(
        string title,
        List<SubCategoryDto> subCategoriesDto,
        int? createdByApplicationUserId,
        int? lastModifiedByApplicationUserId)
    {
        Title = title;
        SubCategoriesDto = subCategoriesDto;
        CreatedByApplicationUserId = createdByApplicationUserId;
        LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
    }

    public string Title { get; set; }

    public List<SubCategoryDto> SubCategoriesDto { get; set; }

    public int? CreatedByApplicationUserId { get; set; }

    public int? LastModifiedByApplicationUserId { get; set; }
}