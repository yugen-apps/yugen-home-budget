using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Shared.Models.Expense;

public class ResponseExpenseDto
{
    public ResponseExpenseDto(
        int id,
        string title,
        decimal amount,
        DateTimeOffset dateTimeOffset,
        ResponseCategoryDto categoryDto,
        SubCategoryDto subCategoryDto,
        DateTimeOffset createdOn, 
        DateTimeOffset lastModifiedOn,
        int? createdByApplicationUserId,
        int? lastModifiedByApplicationUserId)
    {
        Id = id;
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        CategoryDto = categoryDto;
        SubCategoryDto = subCategoryDto;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
        CreatedByApplicationUserId = createdByApplicationUserId;
        LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }


    public ResponseCategoryDto CategoryDto { get; set; }

    public SubCategoryDto SubCategoryDto { get; set; }


    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastModifiedOn { get; set; }

    public int? CreatedByApplicationUserId { get; set; }

    public int? LastModifiedByApplicationUserId { get; set; }
}