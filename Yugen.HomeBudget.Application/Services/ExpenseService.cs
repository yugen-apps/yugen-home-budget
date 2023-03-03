using Yugen.HomeBudget.Application.Extensions;
using Yugen.HomeBudget.Data.Models;
using Yugen.HomeBudget.Data.Repositories;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Application.Services
{
    public class ExpenseService
    {
        private readonly ExpenseRepository _expenseRepository;

        public ExpenseService(ExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<ExpenseDto>> ListAsync()
        {
            var expenses = await _expenseRepository.ListAsync();
            return expenses.Select(e => e.ToDto()).ToList();
        }

        public async Task<PaginatedList<ExpenseDto>> ListAsync(int pageIndex, int pageSize)
        {
            var totalItemCount = await _expenseRepository.CountAsync();
            var skip = (pageIndex - 1) * pageSize;
            var categoriesDto = (await _expenseRepository.ListAsync(skip, pageSize))
                .Select(e => e.ToDto())
                .ToList();

            return new PaginatedList<ExpenseDto>(categoriesDto, totalItemCount, pageIndex, pageSize);
        }

        public async Task<ExpenseDto> GetAsync(int id)
        {
            var expense = await _expenseRepository.GetAsync(id);
            return expense.ToDto();
        }

        public async Task<ExpenseDto?> CreateAsync(CreateExpenseDto createExpenseDto)
        {
            var expense = new Expense(
                createExpenseDto.Title,
                createExpenseDto.Amount,
                createExpenseDto.DateTimeOffset,
                createExpenseDto.CategoryId,
                createExpenseDto.SubCategoryId);

            var expenseResult = await _expenseRepository.CreateAsync(expense);
            return expenseResult.ToDto();
        }

        public async Task<ExpenseDto?> UpdateAsync(int id, UpdateExpenseDto updateExpenseDto)
        {
            var expense = await _expenseRepository.GetAsync(id);
            if (expense == null)
            {
                return null;
            }

            expense.Title = updateExpenseDto.Title;
            expense.Amount = updateExpenseDto.Amount;
            expense.DateTimeOffset = updateExpenseDto.DateTimeOffset;
            expense.CategoryId = updateExpenseDto.CategoryId;
            expense.SubCategoryId = updateExpenseDto.SubCategoryId;

            var expenseResult = await _expenseRepository.UpdateAsync(expense);
            return expenseResult.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _expenseRepository.GetAsync(id);
            if (expense == null)
            {
                return false;
            }

            await _expenseRepository.DeleteAsync(expense);
            return true;
        }
    }
}