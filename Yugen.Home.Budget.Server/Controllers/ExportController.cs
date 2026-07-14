using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Server.Helpers;
using Yugen.Home.Budget.Server.Models.Navigation;

namespace Yugen.Home.Budget.Server.Controllers;

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
        var paginatedList = await _expenseService.ListAsync(0, 0, DateTimeHelper.YearStart(year), DateTimeHelper.YearEnd(year));

        using var package = new ExcelPackage();
        var workSheet = package.Workbook.Worksheets.Add("Sheet1");

        var exportList = paginatedList.Items.Select(x => x.ToExportDto(x.CategoryDto.Title, x.SubCategoryDto.Title));
        workSheet.Cells.LoadFromCollection(exportList, true);

        var excelData = await package.GetAsByteArrayAsync();
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileName = $"{year}-Expenses.xlsx";
        return File(excelData, contentType, fileName);
    }
}
