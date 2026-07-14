using MudBlazor.Extensions;
using System;
using System.Globalization;

namespace Yugen.Home.Budget.Server.Helpers;

public static class DateTimeHelper
{
    private static readonly DateTime _now = DateTime.Now;

    public static readonly DateTime CurrentYearStart = new(_now.Year, 1, 1);
    public static readonly DateTime CurrentYearEnd = CurrentYearStart.AddYears(1);

    public static readonly DateTime PreviousYearStart = CurrentYearStart.AddYears(-1);
    public static readonly DateTime PreviousYearEnd = CurrentYearEnd.AddYears(-1);

    public static readonly DateTime CurrentMonthStart = _now.StartOfMonth(CultureInfo.InvariantCulture);
    public static readonly DateTime CurrentMonthEnd = CurrentMonthStart.AddMonths(1);

    public static readonly DateTime PreviousMonthStart = _now.AddMonths(-1).StartOfMonth(CultureInfo.InvariantCulture);
    public static readonly DateTime PreviousMonthEnd = PreviousMonthStart.AddMonths(1);

    public static DateTime YearStart(int year) => new(year, 1, 1);

    public static DateTime YearEnd(int year) => YearStart(year).AddYears(1);
}
