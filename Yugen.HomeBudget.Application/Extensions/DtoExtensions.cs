using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Application.Extensions;

public static class DtoExtensions
{
    public static ResponseCategoryDto ToDto(this Category category)
    {
        var categoryDto = new ResponseCategoryDto(
            category.Id,
            category.Title,
            category.Icon,
            category.CreatedOn,
            category.LastModifiedOn,
            category.CreatedByApplicationUserId,
            category.LastModifiedByApplicationUserId);

        if (category?.SubCategories == null)
        {
            return categoryDto;
        }

        foreach (var subCategory in category.SubCategories)
        {
            categoryDto.SubCategoriesDto.Add(subCategory.ToDto());
        }

        return categoryDto;
    }

    public static SubCategoryDto ToDto(this SubCategory subCategory) => new(subCategory.Id, subCategory.Title);

    public static ResponseExpenseDto ToDto(this Expense expense)
    {
        var expenseDto = new ResponseExpenseDto(expense.Id, expense.Title, expense.Amount, expense.DateTimeOffset, expense.Category.ToDto(), expense.SubCategory.ToDto(), expense.Accrued, expense.CreatedOn, expense.LastModifiedOn, expense.CreatedByApplicationUserId, expense.LastModifiedByApplicationUserId);
        return expenseDto;
    }

    public static ExportExpenseDto ToExportExpenseDto(this Expense expense)
    {
        var expenseDto = new ExportExpenseDto(expense.Id, expense.Title, expense.Amount, expense.DateTimeOffset, expense.Category.Title, expense.SubCategory.Title, expense.Accrued, expense.CreatedOn, expense.LastModifiedOn, expense.CreatedByApplicationUserId, expense.LastModifiedByApplicationUserId);
        return expenseDto;
    }
}