using System;

namespace Yugen.Home.Budget.Application.Models.Expense;

public class UpdateExpenseDto
{
    public UpdateExpenseDto(
        int id,
        string title,
        decimal amount,
        DateTimeOffset dateTimeOffset,
        int categoryId,
        int subCategoryId,
        decimal accrued,
        int createdByApplicationUserId,
        int lastModifiedByApplicationUserId)
    {
        Id = id;
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        Accrued = accrued;
        CreatedByApplicationUserId = createdByApplicationUserId;
        LastModifiedByApplicationUserId = lastModifiedByApplicationUserId;
    }

    public decimal Accrued { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; }

    public int? CreatedByApplicationUserId { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public int Id { get; set; }

    public int LastModifiedByApplicationUserId { get; set; }

    public int SubCategoryId { get; set; }

    public string Title { get; set; }
}