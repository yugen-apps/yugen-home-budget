namespace Yugen.HomeBudget.Server.Models;

public class Expense
{
    public Expense()
    {
    }

    public Expense(
        int id,
        string title,
        decimal amount,
        DateTimeOffset dateTimeOffset,
        int categoryId,
        int subCategoryId,
        decimal accrued)
    {
        Id = id;
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        Accrued = accrued;
    }

    public decimal Accrued { get; set; }

    public decimal Amount { get; set; }

    public int CategoryId { get; set; } = 1;

    public DateTimeOffset DateTimeOffset { get; set; } = DateTimeOffset.UtcNow;

    public int Id { get; set; }

    public int SubCategoryId { get; set; } = 1;

    public string Title { get; set; } = string.Empty;
}