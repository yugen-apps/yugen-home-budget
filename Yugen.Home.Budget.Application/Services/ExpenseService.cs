using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Home.Budget.Application.Extensions;
using Yugen.Home.Budget.Application.Models;
using Yugen.Home.Budget.Application.Models.Expense;
using Yugen.Home.Budget.Data.Models;
using Yugen.Home.Budget.Data.Repositories;

namespace Yugen.Home.Budget.Application.Services
{
	public class ExpenseService
	{
		private readonly ExpenseRepository _expenseRepository;

		public ExpenseService(ExpenseRepository expenseRepository)
		{
			_expenseRepository = expenseRepository;
		}

		public async Task<List<ResponseExpenseDto>> ListAsync()
		{
			var result = await _expenseRepository.ListAsync(0, 0, 0, 0);

			var expensesDto = result.Items
								.Select(e => e.ToDto())
								.ToList();

			return expensesDto;
		}

		public async Task<PaginatedList<ResponseExpenseDto>> ListAsync(
			int pageIndex,
			int pageSize,
			int month,
			int year)
		{
			var result = await _expenseRepository.ListAsync(pageIndex, pageSize, month, year);

			var expensesDto = result.Items
								.Select(e => e.ToDto())
								.ToList();

			return new PaginatedList<ResponseExpenseDto>(expensesDto, result.TotalCount, pageIndex, pageSize);
		}

		public async Task<decimal> SumAsync(int year, int month)
		{
			return await _expenseRepository.SumAsync(year, month);
		}

		public async Task<decimal> SumAccruedAsync(int year, int month)
		{
			return await _expenseRepository.SumAccruedAsync(year, month);
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

		public async Task<ResponseExpenseDto> GetAsync(int id)
		{
			var expense = await _expenseRepository.GetAsync(id);
			return expense?.ToDto();
		}

		public async Task<ResponseExpenseDto> CreateAsync(CreateExpenseDto createExpenseDto)
		{
			var expense = new Expense(
				createExpenseDto.Title,
				createExpenseDto.Amount,
				createExpenseDto.DateTimeOffset,
				createExpenseDto.CategoryId,
				createExpenseDto.SubCategoryId,
				createExpenseDto.Accrued,
				createExpenseDto.CreatedByApplicationUserId);

			var expenseResult = await _expenseRepository.CreateAsync(expense);
			return expenseResult.ToDto();
		}

		public async Task<ResponseExpenseDto> UpdateAsync(int id, UpdateExpenseDto updateExpenseDto)
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
			expense.Accrued = updateExpenseDto.Accrued;
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