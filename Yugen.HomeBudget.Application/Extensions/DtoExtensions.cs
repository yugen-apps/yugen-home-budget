using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Application.Extensions;

public static class DtoExtensions
{
    public static ResponseCategoryDto ToDto(this Category category)
    {
        var categoryDto = new ResponseCategoryDto(category.Id, category.Title, category.CreatedOn, category.LastModifiedOn, category.CreatedByApplicationUserId, category.LastModifiedByApplicationUserId);

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

    public static ResponseExpenseDto ToDto(this Expense expense)
    {
        var expenseDto = new ResponseExpenseDto(expense.Id, expense.Title, expense.Amount, expense.DateTimeOffset, expense.Category.ToDto(), expense.SubCategory.ToDto(), expense.CreatedOn, expense.LastModifiedOn, expense.CreatedByApplicationUserId, expense.LastModifiedByApplicationUserId);
        return expenseDto;
    }
}