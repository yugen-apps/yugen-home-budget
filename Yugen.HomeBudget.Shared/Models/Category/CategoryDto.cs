namespace Yugen.HomeBudget.Shared.Models.Category;

public class ResponseCategoryDto
{
    public ResponseCategoryDto(
        int id, 
        string title, 
        DateTimeOffset createdOn, 
        DateTimeOffset lastModifiedOn, 
        int? createdByApplicationUserId, 
        int? lastModifiedByApplicationUserId)
    {
        Id = id;
        Title = title;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
        CreatedByApplicationUserId = createdByApplicationUserId;
        LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public List<SubCategoryDto> SubCategoriesDto { get; set; } = new List<SubCategoryDto>();


    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastModifiedOn { get; set; }

    public int? CreatedByApplicationUserId { get; set; }

    public int? LastModifiedByApplicationUserId { get; set; }
}