using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Home.Budget.Application.Extensions;
using Yugen.Home.Budget.Application.Models;
using Yugen.Home.Budget.Application.Models.Category;
using Yugen.Home.Budget.Data.Models;
using Yugen.Home.Budget.Data.Repositories;

namespace Yugen.Home.Budget.Application.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ResponseCategoryDto>> ListAsync()
        {
            var result = await _categoryRepository.ListAsync(0, 0);

            var categoriesDto = result.Items
                                .Select(c => c.ToDto())
                                .ToList();

            return categoriesDto;
        }

        public async Task<PaginatedList<ResponseCategoryDto>> ListAsync(int pageIndex, int pageSize)
        {
            var result = await _categoryRepository.ListAsync(pageIndex, pageSize);

            var categoriesDto = result.Items
                                .Select(c => c.ToDto())
                                .ToList();


            return new PaginatedList<ResponseCategoryDto>(categoriesDto, result.TotalCount, pageIndex, pageSize);
        }

        public async Task<ResponseCategoryDto> GetAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return category?.ToDto();
        }

        public async Task<ResponseCategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var exists = await _categoryRepository.ExistsAsync(createCategoryDto.Title);
            if (exists)
            {
                return null;
            }

            var category = new Category(createCategoryDto.Title, createCategoryDto.Icon, createCategoryDto.CreatedByApplicationUserId);

            foreach (var subCategoryDto in createCategoryDto.SubCategoriesDto)
            {
                category.SubCategories.Add(new SubCategory(subCategoryDto.Title));
            }

            var categoryResult = await _categoryRepository.CreateAsync(category);
            return categoryResult.ToDto();
        }

        public async Task<ResponseCategoryDto> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                return null;
            }

            category.Title = updateCategoryDto.Title;
            category.Icon = updateCategoryDto.Icon;
            category.LastModifiedByApplicationUserId = updateCategoryDto.LastModifiedByApplicationUserId;

            var existingSubCategoryIds = category.SubCategories.Select(x => x.Id).Distinct().ToArray();
            var inputSubCategoryIds = updateCategoryDto.SubCategoriesDto.Select(x => x.Id).Distinct().ToArray();

            var deletedSubCategoryIds = existingSubCategoryIds.Where(x => !inputSubCategoryIds.Contains(x)).ToArray();
            var updatedSubCategories = updateCategoryDto.SubCategoriesDto.Where(x => x.Id != 0).ToArray();
            var newAddedSubCategories = updateCategoryDto.SubCategoriesDto.Where(x => x.Id == 0).ToArray();

            foreach (var deletedSubCategoryId in deletedSubCategoryIds)
            {
                category.SubCategories.Remove(category.SubCategories.First(x => x.Id == deletedSubCategoryId));
            }

            foreach (var updatedSubCategory in updatedSubCategories)
            {
                category.SubCategories.First(x => x.Id == updatedSubCategory.Id).Title = updatedSubCategory.Title;
            }

            foreach (var newAddedSubCategory in newAddedSubCategories)
            {
                category.SubCategories.Add(new SubCategory(newAddedSubCategory.Title));
            }

            var categoryResult = await _categoryRepository.UpdateAsync(category);
            return categoryResult.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                return false;
            }

            await _categoryRepository.DeleteAsync(category);
            return true;
        }
    }
}