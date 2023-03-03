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

        public Task<List<Expense>> ListAsync(int skip, int pageSize)
        {
            return _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.SubCategory)
                .Skip(skip)
                .Take(pageSize)
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
            //expense.UpdatedAt = DateTime.UtcNow;

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

        public Task<int> CountAsync()
        {
            return _context.Expenses.CountAsync();
        }

        public Task<bool> ExistsAsync(string title)
        {
            return _context.Expenses.AnyAsync(x => x.Title == title);
        }
    }
}