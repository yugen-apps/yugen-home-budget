namespace Yugen.HomeBudget.Shared.Models.Expense;

public class ExportExpenseDto
{
    public ExportExpenseDto(
        int id,
        string title,
        decimal amount,
        DateTimeOffset dateTimeOffset,
        string category,
        string subCategory,
        DateTimeOffset createdOn,
        DateTimeOffset lastModifiedOn,
        int? createdByApplicationUserId,
        int? lastModifiedByApplicationUserId)
    {
        Id = id;
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        Category = category;
        SubCategory = subCategory;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
        CreatedByApplicationUserId = createdByApplicationUserId;
        LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public string Category { get; set; }

    public string SubCategory{ get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset LastModifiedOn { get; set; }

    public int? CreatedByApplicationUserId { get; set; }

    public int? LastModifiedByApplicationUserId { get; set; }
}