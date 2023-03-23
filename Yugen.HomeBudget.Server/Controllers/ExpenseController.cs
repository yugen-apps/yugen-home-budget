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

        /// <summary>
        /// GET: Expense
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public Task<IEnumerable<ResponseExpenseDto>> ListAsync()
        {
            return _expenseService.ListAsync();
        }

        [HttpGet]
        public Task<PaginatedList<ResponseExpenseDto>> ListAsync(int pageNumber, int pageSize)
        {
            return _expenseService.ListAsync(pageNumber, pageSize);
        }

        /// <summary>
        /// GET: Expense/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<ResponseExpenseDto> GetAsync(int id)
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

        /// <summary>
        /// DELETE: Expense/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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