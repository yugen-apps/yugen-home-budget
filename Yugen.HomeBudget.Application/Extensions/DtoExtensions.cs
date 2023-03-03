using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Application.Extensions;

public static class DtoExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        var categoryDto = new CategoryDto(category.Id, category.Title);

        if (category.SubCategories != null)
        {
            foreach (var subCategory in category.SubCategories)
            {
                categoryDto.SubCategoriesDto.Add(subCategory.ToDto());
            }

            return categoryDto;
        }

        return categoryDto;
    }

    public static SubCategoryDto ToDto(this SubCategory subCategory) => new(subCategory.Id, subCategory.Title);

    public static ExpenseDto ToDto(this Expense expense)
    {
        var expenseDto = new ExpenseDto(expense.Id, expense.Title, expense.Amount, expense.DateTimeOffset, expense.Category.ToDto(), expense.SubCategory.ToDto());
        return expenseDto;
    }
}