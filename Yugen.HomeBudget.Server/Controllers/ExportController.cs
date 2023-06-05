using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Shared.Contants;

namespace Yugen.HomeBudget.Server.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route($"{EndpointConstants.Prefix}/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly ExpenseService _expenseService;
        
        public ExportController(
            ILogger<ExpenseController> logger,
            ExpenseService expenseService)
        {
            _logger = logger;
            _expenseService = expenseService;
        }

        [HttpGet("{year}/{month}")]
        public async Task<IActionResult> OnGet(int year, int month)
        {
            var list = await _expenseService.ExportAsync(year, month);
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells.LoadFromCollection(list, true);

            var excelData = await package.GetAsByteArrayAsync();
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"{year}{month}-Expenses.xlsx";
            return File(excelData, contentType, fileName);
        }
    }
}