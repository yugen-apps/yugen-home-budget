using System;
using System.Collections.Generic;
using System.Linq;
using Yugen.HomeBudget.Application.Models.Category;

namespace Yugen.HomeBudget.Server.Models.Expense
{
    public class ExpenseModel
    {
        public string Title { get; set; }

        public decimal? Amount { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Accrued { get; set; }

        public ExpenseModel(List<ResponseCategoryDto> categories)
        {
            DateTime = DateTime.Now;

            Categories = categories;
            SelectedCategory = Categories.First();
            SubCategories = SelectedCategory.SubCategoriesDto;
            SelectedSubCategory = SubCategories.First();
        }

        public ExpenseModel(
            List<ResponseCategoryDto> categories,
            string title,
            decimal amount,
            DateTimeOffset dateTimeOffset,
            int categoryId,
            int subCategoryId,
            decimal accrued)
        {
            Title = title;
            Amount = amount;
            DateTime = dateTimeOffset.DateTime;
            Accrued = accrued;

            Categories = categories;
            SelectedCategory = Categories.First(x => x.Id == categoryId);
            SubCategories = SelectedCategory.SubCategoriesDto;
            SelectedSubCategory = SubCategories.First(x => x.Id == subCategoryId);
        }

        public void CategoryChanged(ResponseCategoryDto category)
        {
            SelectedCategory = category;
            SubCategories = SelectedCategory.SubCategoriesDto;
            SelectedSubCategory = SubCategories.First();
        }

        public void SubCategoryChanged(SubCategoryDto subCategory)
        {
            SelectedSubCategory = subCategory;
        }

        public ResponseCategoryDto SelectedCategory { get; set; }

        public List<ResponseCategoryDto> Categories { get; set; } = [];

        public SubCategoryDto SelectedSubCategory { get; set; }

        public List<SubCategoryDto> SubCategories { get; set; } = [];
    }
}