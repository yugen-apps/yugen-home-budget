using Microsoft.EntityFrameworkCore;
using Yugen.HomeBudget.Data.Models;

namespace Yugen.HomeBudget.Data.Repositories
{
    public class ExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Expense>> ListAsync()
        {
            return _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.SubCategory)
                .ToListAsync();
        }

        public Task<List<Expense>> ListAsync(int year, int month, int skip, int pageSize)
        {
            return _context.Expenses
                .Where(x => x.DateTimeOffset.Month == month &&
                            x.DateTimeOffset.Year == year)
                .Include(e => e.Category)
                .Include(e => e.SubCategory)
                .OrderByDescending(x => x.DateTimeOffset)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<decimal> SumAsync(int year, int month)
        {
            if (month == 0)
            {
                return _context.Expenses
                    .Where(x => x.DateTimeOffset.Year == year)
                    .SumAsync(x => x.Amount);
            }

            return _context.Expenses
                .Where(x => x.DateTimeOffset.Month == month &&
                            x.DateTimeOffset.Year == year)
                .SumAsync(x => x.Amount);
        }

        public Task<List<IGrouping<Category, Expense>>> GroupedByCategoryAsync(int year, int month)
        {
            return _context.Expenses
                .Where(x => x.DateTimeOffset.Month == month &&
                            x.DateTimeOffset.Year == year)
                .Include(e => e.Category)
                .GroupBy(e => e.Category)
                .ToListAsync();
        }

        public async Task<Expense?> GetAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return null;
            }

            await _context.Entry(expense)
                .Reference(e => e.Category)
                .LoadAsync();

            await _context.Entry(expense)
                .Reference(e => e.SubCategory)
                .LoadAsync();

            return expense;
        }

        public async Task<Expense> CreateAsync(Expense expense)
        {
            expense.CreatedOn = DateTimeOffset.UtcNow;

            _context.Expenses.Add(expense);

            await _context.Entry(expense)
                .Reference(e => e.Category)
                .LoadAsync();

            await _context.Entry(expense)
                .Reference(e => e.SubCategory)
                .LoadAsync();

            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task<Expense> UpdateAsync(Expense expense)
        {
            expense.LastModifiedOn = DateTimeOffset.UtcNow;

            _context.Expenses.Update(expense);

            await _context.Entry(expense)
                .Reference(e => e.Category)
                .LoadAsync();

            await _context.Entry(expense)
                .Reference(e => e.SubCategory)
                .LoadAsync();

            await _context.SaveChangesAsync();

            return expense;
        }

        public async Task DeleteAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);

            await _context.SaveChangesAsync();
        }

        public Task<int> CountAsync(int year, int month)
        {
            return _context.Expenses
                .Where(x => x.DateTimeOffset.Month == month &&
                            x.DateTimeOffset.Year == year)
                .CountAsync();
        }

        public Task<bool> ExistsAsync(string title)
        {
            return _context.Expenses.AnyAsync(x => x.Title == title);
        }
    }
}