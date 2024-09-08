using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Server.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route($"{EndpointConstants.Prefix}/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly ExpenseService _expenseService;

        public ExpenseController(
            ILogger<ExpenseController> logger,
            ExpenseService expenseService)
        {
            _logger = logger;
            _expenseService = expenseService;
        }

        [HttpGet]
        [Route("all")]
        public Task<IEnumerable<ResponseExpenseDto>> ListAsync()
        {
            return _expenseService.ListAsync();
        }
        
        [HttpGet]
        public Task<PaginatedList<ResponseExpenseDto>> ListAsync(int year, int month, int pageNumber, int pageSize)
        {
            return _expenseService.ListAsync(year, month, pageNumber, pageSize);
        }

        [HttpGet]
        [Route("sum")]
        public Task<decimal> SumAsync(int year, int month)
        {
            return _expenseService.SumAsync(year, month);
        }     
        
        [HttpGet]
        [Route("sumaccrued")]
        public Task<decimal> SumAccruedAsync(int year, int month)
        {
            return _expenseService.SumAccruedAsync(year, month);
        }

        [HttpGet]
        [Route("groupedbycategory")]
        public Task<List<ResponseExpenseGroupedByCategoryDto>> GroupedByCategoryAsync(int year, int month)
        {
            return _expenseService.GroupedByCategoryAsync(year, month);
        }
        
        [HttpGet("{id}")]
        public Task<ResponseExpenseDto?> GetAsync(int id)
        {
            return _expenseService.GetAsync(id);
        }

        /// <summary>
        /// POST: Expense
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// </summary>
        /// <param name="createExpenseDto"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ResponseExpenseDto?> CreateAsync(CreateExpenseDto createExpenseDto)
        {
            return _expenseService.CreateAsync(createExpenseDto);
        }

        /// <summary>
        /// PUT: Expense/5
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateExpenseDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ResponseExpenseDto?> UpdateAsync(int id, UpdateExpenseDto updateExpenseDto)
        {
            //if (id != editExpenseDto.Id)
            //{
            //    return BadRequest();
            //}

            return await _expenseService.UpdateAsync(id, updateExpenseDto);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var result = await _expenseService.DeleteAsync(id);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}