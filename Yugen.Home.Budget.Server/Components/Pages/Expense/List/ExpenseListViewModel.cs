using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Components;
using Yugen.Common.Blazor.Components.LoadingSpinner;
using Yugen.Home.Budget.Application.Models;
using Yugen.Home.Budget.Application.Models.Expense;
using Yugen.Home.Budget.Application.Services;
using Yugen.Home.Budget.Server.Helpers;

namespace Yugen.Home.Budget.Server.ViewModels.Expense;

public sealed partial class ExpenseListViewModel : ObservableObject
{
    private readonly ExpenseService _expenseService;
    private readonly IDialogService _dialogService;
    private readonly ILoadingSpinnerService _loadingSpinnerService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial PaginatedList<ResponseExpenseDto> PaginatedList { get; set; } = new();

    [ObservableProperty]
    public partial DateRange DateRange { get; set; } = new DateRange(DateTimeHelper.CurrentMonthStart, DateTimeHelper.CurrentMonthEnd);

    public ExpenseListViewModel(
        ExpenseService expenseService,
        IDialogService dialogService,
        ILoadingSpinnerService loadingSpinnerService)
    {
        _expenseService = expenseService;
        _dialogService = dialogService;
        _loadingSpinnerService = loadingSpinnerService;
    }

    public Func<Task> RefreshServerDataFunc { get; set; }

    public async Task<GridData<ResponseExpenseDto>> ServerReload(GridState<ResponseExpenseDto> state, CancellationToken _)
    {
        await RefreshDataAsync(state.Page, state.PageSize);

        return new GridData<ResponseExpenseDto>
        {
            TotalItems = PaginatedList.TotalItems,
            Items = PaginatedList.Items
        };
    }

    public Task DateRangeChanged()
    {
        return RefreshServerDataFunc.Invoke();
    }

    public async Task ShowDeleteConfirmMessage(int id)
    {
        var parameters = new DialogParameters<MessageDialog>
        {
            { x => x.ContentText, "Do you really want to delete these records? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var dialog = await _dialogService.ShowAsync<MessageDialog>("Delete", parameters);

        var result = await dialog.Result;
        if ((result?.Canceled) != false)
        {
            return;
        }

        await DeleteAsync(id);
    }

    private async Task DeleteAsync(int id)
    {
        var result = await _expenseService.DeleteAsync(id);
        if (!result)
        {
            return;
        }

        var deletedItem = PaginatedList.Items.FirstOrDefault(x => x.Id == id);
        if (deletedItem == null)
        {
            return;
        }

        PaginatedList.Remove(deletedItem);
    }

    public async Task RefreshDataAsync(int page, int pageSize)
    {
        _loadingSpinnerService.Wait();
        IsLoading = true;

        try
        {

            PaginatedList = await _expenseService.ListAsync(page, pageSize, DateRange.Start, DateRange.End) ?? new();
        }
        finally
        {
            IsLoading = false;
            _loadingSpinnerService.Resume();
        }
    }
}