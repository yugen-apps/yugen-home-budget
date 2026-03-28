using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Threading.Tasks;
using Yugen.Home.Budget.Application.Services;
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
        var list = await _expenseService.ListAsync(0, 0, 0, year);

        using var package = new ExcelPackage();
        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
        workSheet.Cells.LoadFromCollection(list.Items, true);

        var excelData = await package.GetAsByteArrayAsync();
        const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileName = $"{year}-Expenses.xlsx";
        return File(excelData, contentType, fileName);
    }
}