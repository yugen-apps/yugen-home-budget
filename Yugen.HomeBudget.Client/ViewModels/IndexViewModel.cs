using Blazorise;
using Blazorise.Charts;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Models;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.ViewModels;

public sealed partial class IndexViewModel : ObservableObject
{
    public PieChart<double>? PieChart;
    private readonly HttpClient _httpClient;
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

    public IndexViewModel(
        HttpClient httpClient, 
        IMessageService messageService)
    {
        _httpClient = httpClient;
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
            var currentMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={Year}&month={Month}");
            CurrentMonthTotalExpense = new TotalExpense(currentMonthExpenseTotal, PreviousMonthTotalExpense?.Current ?? 0);
        }
        catch { }
    }

    public async Task LoadCurrentYearTotalAccrued()
    {
        try
        {
            var currentYearTotalAccrued = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.AccruedSum}?year={Year}");
            CurrentYearTotalAccrued = new TotalAccrued(currentYearTotalAccrued);
        }
        catch { }
    }

    public async Task LoadCurrentYearTotalExpense()
    {
        try
        {
            var currentYearTotalExpense = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={Year}");
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
            var data = await _httpClient.GetFromJsonAsync<List<ResponseExpenseGroupedByCategoryDto>>($"{EndpointConstants.GroupedByCategory}?year={Year}&month={Month}");
            if (data != null)
            {
                var labels = data.Select(x => x.Category).ToList();
                var values = data.Select(x => (double)x.Total).ToList();
                var colors = data.Select(x => Constants.ChartColors[x.Index]).ToList();

                var currentMonthAccruedTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.AccruedSum}?year={Year}&month={Month}");
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
            var previousMonthExpenseTotal = await _httpClient.GetFromJsonAsync<decimal>($"{EndpointConstants.ExpenseSum}?year={Year}&month={Month - 1}");
            PreviousMonthTotalExpense = new TotalExpense(previousMonthExpenseTotal, CurrentMonthTotalExpense?.Current ?? 0);
        }
        catch { }
    }

    public async Task OnReadData(DataGridReadDataEventArgs<ResponseExpenseDto> e)
    {
        try
        {
            if (!e.CancellationToken.IsCancellationRequested)
            {
                var response = await _httpClient.GetFromJsonAsync<PaginatedList<ResponseExpenseDto>>($"{EndpointConstants.Expense}?year={Year}&month={Month}&pageNumber={e.Page}&pageSize={e.PageSize}");
                if (response != null &&
                    !e.CancellationToken.IsCancellationRequested)
                {
                    PaginatedList = response;
                }
            }
        }
        catch
        {
        }
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
        var result = await _httpClient.DeleteAsync($"{EndpointConstants.Expense}/{id}");
        if (result.IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }
}