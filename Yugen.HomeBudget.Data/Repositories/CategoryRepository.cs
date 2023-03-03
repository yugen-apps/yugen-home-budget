using Microsoft.EntityFrameworkCore;
using Yugen.HomeBudget.Data.Models;

namespace Yugen.HomeBudget.Data.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Category>> ListAsync()
        {
            return _context.Categories
                .Include(category => category.SubCategories)
                .ToListAsync();
        }

        public Task<List<Category>> ListAsync(int skip, int pageSize)
        {
            return _context.Categories
                .Include(category => category.SubCategories)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Category?> GetAsync(int id)
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
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            //category.UpdatedAt = DateTime.UtcNow;

            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();
        }

        public Task<int> CountAsync()
        {
            return _context.Categories.CountAsync();
        }

        public Task<bool> ExistsAsync(string title)
        {
            return _context.Categories.AnyAsync(x => x.Title == title);
        }
    }
}