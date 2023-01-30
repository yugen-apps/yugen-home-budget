using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Server.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route($"{EndpointConstants.Prefix}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoryService _categoryService;

        public CategoryController(
            ILogger<CategoryController> logger,
            CategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }
        
        [HttpGet]
        [Route("all")]
        public Task<IEnumerable<ResponseCategoryDto>> ListAsync()
        {
            return _categoryService.ListAsync();
        }

        [HttpGet]
        public Task<PaginatedList<ResponseCategoryDto>> ListAsync(int pageNumber, int pageSize)
        {
            return _categoryService.ListAsync(pageNumber, pageSize);
        }
        
        [HttpGet("{id}")]
        public Task<ResponseCategoryDto?> GetAsync(int id)
        {
            return _categoryService.GetAsync(id);
        }

        /// <summary>
        /// POST: Category
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// </summary>
        /// <param name="createCategoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ResponseCategoryDto?> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            return _categoryService.CreateAsync(createCategoryDto);
        }

        /// <summary>
        /// PUT: Category/5
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateCategoryDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public Task<ResponseCategoryDto?> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            //if (id != updateCategoryDto.Id)
            //{
            //    return BadRequest();
            //}

            return _categoryService.UpdateAsync(id, updateCategoryDto);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}