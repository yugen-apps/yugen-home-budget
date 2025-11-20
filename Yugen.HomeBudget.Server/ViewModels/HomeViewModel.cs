using Blazorise;
using Blazorise.Charts;
using Blazorise.DataGrid;
using CommunityToolkit.Mvvm.ComponentModel;
using Yugen.HomeBudget.Application.Services;
using Yugen.HomeBudget.Server.Models;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Server.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
{
    public PieChart<double>? PieChart;
    private readonly CategoryService _categoryService;
    private readonly ExpenseService _expenseService;
    private readonly IMessageService _messageService;

    [ObservableProperty]
    private TotalExpense? _currentMonthTotalExpense;

    [ObservableProperty]
    private TotalAccrued? _currentYearTotalAccrued;

    [ObservableProperty]
    private TotalExpense? _currentYearTotalExpense;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private int _month = DateTimeOffset.UtcNow.Month;

    [ObservableProperty]
    private PaginatedList<ResponseExpenseDto> _paginatedList = new();

    [ObservableProperty]
    private TotalExpense? _previousMonthTotalExpense;

    [ObservableProperty]
    private int _year = DateTimeOffset.UtcNow.Year;

    public HomeViewModel(
        CategoryService categoryService,
        ExpenseService expenseService,
        IMessageService messageService)
    {
        _categoryService = categoryService;
        _expenseService = expenseService;
        _messageService = messageService;
    }

    public ICollection<ResponseExpenseDto> Expenses => PaginatedList.Items;

    public async Task DateChanged()
    {
        await LoadDataAsync();
    }

    public async Task LoadCurrentMonthTotalExpense()
    {
        try
        {
            //var currentMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={Year}&month={Month}");
            var currentMonthExpenseTotal = await _expenseService.SumAsync(Year, Month);
            CurrentMonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, PreviousMonthTotalExpense?.Current ?? 0);
        }
        catch { }
    }

    public async Task LoadCurrentYearTotalAccrued()
    {
        try
        {
            //var currentYearTotalAccrued = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.AccruedSum}?year={Year}");
            var currentYearTotalAccrued = await _expenseService.SumAccruedAsync(Year, 0);
            CurrentYearTotalAccrued = new TotalAccrued(currentYearTotalAccrued);
        }
        catch { }
    }

    public async Task LoadCurrentYearTotalExpense()
    {
        try
        {
            //var currentYearTotalExpense = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={Year}");
            var currentYearTotalExpense = await _expenseService.SumAsync(Year, 0);
            CurrentYearTotalExpense = new TotalExpense(currentYearTotalExpense);
        }
        catch { }
    }

    public async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            await LoadCurrentYearTotalExpense();
            await LoadPreviousMonthTotalExpense();
            await LoadCurrentYearTotalAccrued();
            await LoadCurrentMonthTotalExpense();
        }
        //catch (AccessTokenNotAvailableException exception)
        //exception.Redirect();
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LoadPieChartData()
    {
        try
        {
            //var data = await _httpClient.GetFromJsonAsync<List<ResponseExpenseGroupedByCategoryDto>>($"{EndpointConstants.GroupedByCategory}?year={Year}&month={Month}");
            var data = await _expenseService.GroupedByCategoryAsync(Year, Month);
            if (data != null)
            {
                var labels = data.Select(x => x.Category).ToList();
                var values = data.Select(x => (double)x.Total).ToList();
                var colors = data.Select(x => Constants.ChartColors[x.Index]).ToList();

                //var currentMonthAccruedTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.AccruedSum}?year={Year}&month={Month}");
                var currentMonthAccruedTotal = await _expenseService.SumAccruedAsync(Year, Month);
                labels.Add("Accrued");
                values.Add((double)currentMonthAccruedTotal);
                colors.Add(Constants.ChartColors[colors.Count]);

                var pieChartDataset = new PieChartDataset<double>
                {
                    Label = "Expenses",
                    Data = values,
                    BackgroundColor = colors,
                    BorderColor = colors,
                };

                await HandleRedraw(labels.ToArray(), pieChartDataset);
            }
        }
        catch { }
    }

    public async Task LoadPreviousMonthTotalExpense()
    {
        try
        {
            //var previousMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={Year}&month={Month - 1}");
            var previousMonthExpenseTotal = await _expenseService.SumAsync(Year, Month - 1);
            PreviousMonthTotalExpense = new TotalExpense(previousMonthExpenseTotal, CurrentMonthTotalExpense?.Current ?? 0);
        }
        catch { }
    }

    public async Task OnReadData(int page, int pageSize)
    {
        try
        {
            PaginatedList = await _expenseService.ListAsync(Year, Month, page, pageSize) ?? new();
            //var response = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={Year}&month={Month}&pageNumber={page}&pageSize={pageSize}");
            //PaginatedList = response ?? new();
        }
        catch
        {
        }
    }

    public async Task OnRowRemoving(CancellableRowChange<ResponseExpenseDto> e)
    {
        e.Cancel = await ShowDeleteConfirmMessage(e.OldItem.Id);
    }

    public async Task<bool> ShowDeleteConfirmMessage(int id)
    {
        var confirmed = await _messageService.Confirm("Are you sure you want to delete?", "Confirmation");
        if (confirmed)
        {
            return await DeleteAsync(id);
        }
        return true;
    }

    private async Task<bool> DeleteAsync(int id)
    {
        //var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
        var result = await _expenseService.DeleteAsync(id);
        if (result)
        {
            return false;
        }
        return true;
    }

    private async Task HandleRedraw(string[] labels, PieChartDataset<double> pieChartDataset)
    {
        if (PieChart == null)
        {
            return;
        }

        await PieChart.Clear();

        await PieChart.AddLabelsDatasetsAndUpdate(labels, pieChartDataset);
    }
}