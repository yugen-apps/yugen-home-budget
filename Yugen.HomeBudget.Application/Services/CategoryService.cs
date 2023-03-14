using Yugen.HomeBudget.Application.Extensions;
using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Data.Repositories;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Application.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> ListAsync()
        {
            var categories = await _categoryRepository.ListAsync();
            return categories.Select(c => c.ToDto()).ToList();
        }

        public async Task<PaginatedList<CategoryDto>> ListAsync(int pageIndex, int pageSize)
        {
            var totalItemCount = await _categoryRepository.CountAsync();
            var skip = (pageIndex - 1) * pageSize;
            var categoriesDto = (await _categoryRepository.ListAsync(skip, pageSize))
                                .Select(c => c.ToDto())
                                .ToList();
            
            return new PaginatedList<CategoryDto>(categoriesDto, totalItemCount, pageIndex, pageSize);
        }

        public async Task<CategoryDto> GetAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            return category.ToDto();
        }

        public async Task<CategoryDto?> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var exists = await _categoryRepository.ExistsAsync(createCategoryDto.Title);
            if (exists)
            {
                return null;
            }

            var category = new Category(createCategoryDto.Title, createCategoryDto.CreatedByApplicationUserId);

            foreach (var subCategoryDto in createCategoryDto.SubCategoriesDto)
            {
                category.SubCategories.Add(new SubCategory(subCategoryDto.Title));
            }

            var categoryResult = await _categoryRepository.CreateAsync(category);
            return categoryResult.ToDto();
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null)
            {
                return null;
            }

            category.Title = updateCategoryDto.Title;
            category.LastModifiedByApplicationUserId = updateCategoryDto.LastModifiedByApplicationUserId;

            var existingSubCategoryIds = category.SubCategories.Select(x => x.Id).Distinct();
            var inputSubCategoryIds = updateCategoryDto.SubCategoriesDto.Select(x => x.Id).Distinct();

            // deleted sub categories
            var deletedSubCategoryIds = existingSubCategoryIds.Where(x => !inputSubCategoryIds.Contains(x));

            foreach (var deletedSubCategoryId in deletedSubCategoryIds)
            {
                category.SubCategories.Remove(category.SubCategories.First(x => x.Id == deletedSubCategoryId));
            }

            // updated sub categories
            var updatedSubCategories = updateCategoryDto.SubCategoriesDto.Where(x => x.Id != 0);

            foreach (var updatedSubCategory in updatedSubCategories)
            {
                category.SubCategories.First(x => x.Id == updatedSubCategory.Id).Title = updatedSubCategory.Title;
            }

            // newly added sub categories
            var newAddedSubCategories = updateCategoryDto.SubCategoriesDto.Where(x => x.Id == 0);

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