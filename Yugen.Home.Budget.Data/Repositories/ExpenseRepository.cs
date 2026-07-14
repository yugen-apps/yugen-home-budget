using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Home.Budget.Data.Models;

namespace Yugen.Home.Budget.Data.Repositories
{
    public class ExpenseRepository : BaseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseListDto<Expense>> ListAsync(
            int page,
            int pageSize,
            DateTime? start,
            DateTime? end)
        {
            var query = _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.SubCategory)
                .OrderByDescending(x => x.DateTimeOffset)
                .AsQueryable();

            if (start.HasValue &&
                end.HasValue)
            {
                query = FilterResults(query, start.Value, end.Value);
            }

            var pagedResults = await GetListAsync(query, page, pageSize);

            return pagedResults;
        }

        public async Task<decimal> SumAsync(DateTime start, DateTime end)
        {
            var query = _context.Expenses.AsQueryable();

            query = FilterResults(query, start, end);

            return await query.SumAsync(x => x.Amount);
        }

        public async Task<decimal> SumAccruedAsync(DateTime start, DateTime end)
        {
            var query = _context.Expenses.AsQueryable();

            query = FilterResults(query, start, end);

            return await query.SumAsync(x => x.Accrued);
        }

        public async Task<List<IGrouping<Category, Expense>>> GroupedByCategoryAsync(DateTime start, DateTime end)
        {
            var query = _context.Expenses.AsQueryable();

            query = FilterResults(query, start, end);

            return await query
                .Include(e => e.Category)
                .GroupBy(e => e.Category)
                .ToListAsync();
        }

        public async Task<Expense> GetAsync(int id)
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
            expense.LastModifiedOn = DateTimeOffset.UtcNow;

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

        public async Task<int> CountAsync(int year, int month)
        {
            return await _context.Expenses
                .Where(x => x.DateTimeOffset.Month == month &&
                            x.DateTimeOffset.Year == year)
                .CountAsync();
        }

        public async Task<bool> ExistsAsync(string title)
        {
            return await _context.Expenses.AnyAsync(x => x.Title == title);
        }

        private IQueryable<Expense> FilterResults(IQueryable<Expense> query, DateTime start, DateTime end)
        {
            query = query.Where(x => x.DateTimeOffset >= start.Date &&
                                        x.DateTimeOffset <= end.Date);

            return query;
        }
    }
}