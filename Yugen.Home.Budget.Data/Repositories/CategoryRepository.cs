using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Home.Budget.Data.Models;

namespace Yugen.Home.Budget.Data.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseListDto<Category>> ListAsync(int page, int pageSize)
        {
            var query = _context.Categories
                .Include(category => category.SubCategories.OrderBy(subcategory => subcategory.Title))
                .OrderBy(category => category.Title)
                .AsQueryable();

            var pagedResults = await GetListAsync(query, page, pageSize);

            return pagedResults;
        }

        public async Task<Category> GetAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return null;
            }

            await _context.Entry(category)
                .Collection(c => c.SubCategories)
                .LoadAsync();

            return category;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            category.CreatedOn = DateTimeOffset.UtcNow;
            category.LastModifiedOn = DateTimeOffset.UtcNow;

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            category.LastModifiedOn = DateTimeOffset.UtcNow;

            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<bool> ExistsAsync(string title)
        {
            return await _context.Categories.AnyAsync(x => x.Title == title);
        }
    }
}