using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Server.Navigation;

namespace Yugen.HomeBudget.Server.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route($"{MenuConstants.ApiPrefix}/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly ILogger<ExportController> _logger;
        private readonly ExpenseService _expenseService;

        public ExportController(
            ILogger<ExportController> logger,
            ExpenseService expenseService)
        {
            _logger = logger;
            _expenseService = expenseService;
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> OnGet(int year)
        {
            var list = await _expenseService.ExportAsync(year);
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(list, true);

            var excelData = await package.GetAsByteArrayAsync();
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"{year}-Expenses.xlsx";
            return File(excelData, contentType, fileName);
        }
    }
}