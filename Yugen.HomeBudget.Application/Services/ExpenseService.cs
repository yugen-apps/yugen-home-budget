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

        public async Task<IEnumerable<ResponseExpenseDto>> ListAsync()
        {
            var expenses = await _expenseRepository.ListAsync();
            return expenses.Select(e => e.ToDto()).ToList();
        }

        public async Task<IEnumerable<ResponseExpenseDto>> ListAsync(int year, int month)
        {
            var expenses = await _expenseRepository.ListAsync(year, month);
            return expenses.Select(e => e.ToDto()).ToList();
        }

        public async Task<IEnumerable<ExportExpenseDto>> ExportAsync(int year, int month)
        {
            var expenses = await _expenseRepository.ListAsync(year, month);
            return expenses.Select(e => e.ToExportExpenseDto()).ToList();
        }

        public async Task<PaginatedList<ResponseExpenseDto>> ListAsync(int year, int month, int pageIndex, int pageSize)
        {
            var totalItemCount = await _expenseRepository.CountAsync(year, month);
            var skip = (pageIndex - 1) * pageSize;
            var categoriesDto = (await _expenseRepository.ListAsync(year, month, skip, pageSize))
                .Select(e => e.ToDto())
                .ToList();

            return new PaginatedList<ResponseExpenseDto>(categoriesDto, totalItemCount, pageIndex, pageSize);
        }

        public Task<decimal> SumAsync(int year, int month)
        {
            return _expenseRepository.SumAsync(year, month);
        }

        public async Task<List<ResponseExpenseGroupedByCategoryDto>> GroupedByCategoryAsync(int year, int month)
        {
            var response = await _expenseRepository.GroupedByCategoryAsync(year, month);
            var groupedList = new List<ResponseExpenseGroupedByCategoryDto>();

            foreach (var item in response)
            {
                var title = item.Key.Title;
                var total = item.Key.Expenses.Sum(e => e.Amount);
                var index = groupedList.Count;
                groupedList.Add(new ResponseExpenseGroupedByCategoryDto(title, (int)total, index));
            }

            return groupedList;
        }

        public async Task<ResponseExpenseDto?> GetAsync(int id)
        {
            var expense = await _expenseRepository.GetAsync(id);
            return expense?.ToDto();
        }

        public async Task<ResponseExpenseDto?> CreateAsync(CreateExpenseDto createExpenseDto)
        {
            var expense = new Expense(
                createExpenseDto.Title,
                createExpenseDto.Amount,
                createExpenseDto.DateTimeOffset,
                createExpenseDto.CategoryId,
                createExpenseDto.SubCategoryId,
                createExpenseDto.CreatedByApplicationUserId);

            var expenseResult = await _expenseRepository.CreateAsync(expense);
            return expenseResult.ToDto();
        }

        public async Task<ResponseExpenseDto?> UpdateAsync(int id, UpdateExpenseDto updateExpenseDto)
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
            expense.LastModifiedByApplicationUserId = updateExpenseDto.LastModifiedByApplicationUserId;

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