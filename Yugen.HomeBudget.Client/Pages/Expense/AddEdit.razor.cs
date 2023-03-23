using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Pages.Expense
{
    public partial class AddEdit
    {
        /// <summary>
        /// Avoid concurrent requests
        /// </summary>
        private bool _busy;

        /// <summary>
        /// An error occurred in the update
        /// </summary>
        private bool _error;

        /// <summary>
        /// A concurrency error needs resolution
        /// </summary>
        private bool _concurrencyError;

        /// <summary>
        /// Error message
        /// </summary>
        private string _errorMessage = string.Empty;

        private List<ResponseCategoryDto>? _categories;

        /// <summary>
        /// Id of entity to edit
        /// </summary>
        [Parameter]
        public int? Id { get; set; }

        [Inject]
        private HttpClient _httpClient { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private CustomStateProvider _authStateProvider { get; set; }

        /// <summary>
        /// Expense entity.
        /// </summary>
        private Yugen.HomeBudget.Client.Models.Expense? Expense { get; set; }

        private CurrentUser? _currentUser { get; set; }

        /// <summary>
        /// Start it up
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnInitializedAsync()
        {
            _busy = true;

            _currentUser = await _authStateProvider.GetCurrentUser();

            try
            {
                _categories = await _httpClient.GetFromJsonAsync<List<ResponseCategoryDto>>($"{EndpointConstants.Category}/all");
                if (Id != null)
                {
                    await LoadAsync();
                }
                else
                {
                    Expense = new(0, "", 0, DateTimeOffset.UtcNow, 1, 1);
                }
            }
            finally
            {
                _busy = false;
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Loads the entity
        /// </summary>
        /// <returns>Task</returns>
        private async Task LoadAsync()
        {
            Expense = null;

            //try
            //{
                var expenseDto = await _httpClient.GetFromJsonAsync<ResponseExpenseDto>($"{EndpointConstants.Expense}/{Id}");
                Expense = new(expenseDto.Id, expenseDto.Title, expenseDto.Amount, expenseDto.DateTimeOffset, expenseDto.CategoryDto.Id, expenseDto.SubCategoryDto.Id);
            //}
            //catch (AccessTokenNotAvailableException exception)
            //{
            //    exception.Redirect();
            //}
        }

        /// <summary>
        /// Ask to cancel.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        private void CancelAsync()
        {
            _busy = true;
            _navigationManager.NavigateTo(PageConstants.ExpenseUrl);
        }

        /// <summary>
        /// Handle form submission request.
        /// </summary>
        /// <param name="isValid"><c>True</c> when field validation passed.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private async Task SubmitAsync(bool isValid)
        {
            if (_busy)
            {
                return;
            }

            if (!isValid)
            {
                // still need to edit model
                _error = false;
                _concurrencyError = false;
                return;
            }

            _busy = true; // async
            try
            {
                if (Id != null)
                {
                    var createExpenseDto = new CreateExpenseDto(Expense.Title, Expense.Amount, Expense.DateTimeOffset, Expense.CategoryId, Expense.SubCategoryId, _currentUser?.Id, _currentUser?.Id);
                    var response = await _httpClient.PutAsJsonAsync<CreateExpenseDto>($"{EndpointConstants.Expense}/{Id}", createExpenseDto);
                }
                else
                {
                    var updateExpenseDto = new UpdateExpenseDto(Expense.Id, Expense.Title, Expense.Amount, Expense.DateTimeOffset, Expense.CategoryId, Expense.SubCategoryId, _currentUser?.Id, _currentUser?.Id);
                    var response = await _httpClient.PostAsJsonAsync<UpdateExpenseDto>($"{EndpointConstants.Expense}", updateExpenseDto);
                }

                EditSuccessState.Success = true;
                // go to view to see the record
                _navigationManager.NavigateTo(PageConstants.ExpenseUrl);
            }
            catch (Exception ex)
            {
                EditSuccessState.Success = false;
                // unknown exception
                _error = true;
                _errorMessage = ex.Message;
                _busy = false;
            }
        }
    }
}