namespace Yugen.HomeBudget.Shared.Models.Expense;

public class CreateExpenseDto
{
    public CreateExpenseDto(
        string title,
        decimal amount,
        DateTimeOffset dateTimeOffset,
        int categoryId,
        int subCategoryId)
    {
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
    }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }
}