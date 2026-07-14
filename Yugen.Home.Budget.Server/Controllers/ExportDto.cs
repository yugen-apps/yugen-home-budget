using System;

namespace Yugen.Home.Budget.Server.Controllers;

public class ExportDto
{
    public DateTimeOffset DateTime { get; set; }

    public string Category { get; set; }

    public string SubCategory { get; set; }

    public decimal Amount { get; set; }

    public decimal Accrued { get; set; }

    public string Title { get; set; }

    public int Id { get; set; }
}
