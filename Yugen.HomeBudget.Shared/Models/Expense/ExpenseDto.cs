using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Shared.Models.Expense;

public class ExpenseDto
{
    public ExpenseDto(
        int id, 
        string title, 
        decimal amount, 
        DateTimeOffset dateTimeOffset, 
        CategoryDto categoryDto, 
        SubCategoryDto subCategoryDto)
    {
        Id = id;
        Title = title;
        Amount = amount;
        DateTimeOffset = dateTimeOffset;
        //CategoryId = categoryId;
        //SubCategoryId = subCategoryId;
        CategoryDto = categoryDto;
        SubCategoryDto = subCategoryDto;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    //public int CategoryId { get; set; }

    //public int SubCategoryId { get; set; }

    public CategoryDto CategoryDto { get; set; }

    public SubCategoryDto SubCategoryDto { get; set; }
}