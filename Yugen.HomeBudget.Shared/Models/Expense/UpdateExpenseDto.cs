namespace Yugen.HomeBudget.Shared.Models.Expense;

public class UpdateExpenseDto
{
    public UpdateExpenseDto(
        int id,
        string title,
        decimal amount,
        DateTimeOffset dateTimeOffset,
        int categoryId,
        int subCategoryId)
    {
        Id = id;
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }
}