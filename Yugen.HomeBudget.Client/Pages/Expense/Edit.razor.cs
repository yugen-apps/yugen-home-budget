using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Pages.Expense
{
    public partial class Edit
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

        /// <summary>
        /// Id of entity to edit
        /// </summary>
        [Parameter]
        public int? Id { get; set; }

        [Inject]
        private HttpClient _httpClient { get; set; }

        [Inject]
        private NavigationManager _navigation { get; set; }

        /// <summary>
        /// Expense entity.
        /// </summary>
        private ExpenseDto? ExpenseDto { get; set; }

        private CategoryDto[]? _categories;

        private CategoryDto? _selectedCategory;

        /// <summary>
        /// Start it up
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnInitializedAsync()
        {
            _busy = true;

            try
            {
                _categories = await _httpClient.GetFromJsonAsync<CategoryDto[]>("Category");
                if (Id != null)
                {
                    await LoadAsync();
                }
                else
                {
                    ExpenseDto = new ExpenseDto(0, "", 0, DateTimeOffset.UtcNow,  new CategoryDto(1, null), new SubCategoryDto(1, null));
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
            ExpenseDto = null;

            //try
            //{
                ExpenseDto = await _httpClient.GetFromJsonAsync<ExpenseDto>($"expense/{Id}");
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
            _navigation.NavigateTo("/expense/list");
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
                    var createExpenseDto = new CreateExpenseDto(ExpenseDto.Title, ExpenseDto.Amount, ExpenseDto.DateTimeOffset, ExpenseDto.CategoryDto.Id, ExpenseDto.SubCategoryDto.Id);
                    var response = await _httpClient.PutAsJsonAsync<CreateExpenseDto>($"expense/{Id}", createExpenseDto);
                }
                else
                {
                    var updateExpenseDto = new UpdateExpenseDto(ExpenseDto.Id, ExpenseDto.Title, ExpenseDto.Amount, ExpenseDto.DateTimeOffset, ExpenseDto.CategoryDto.Id, ExpenseDto.SubCategoryDto.Id);
                    var response = await _httpClient.PostAsJsonAsync<UpdateExpenseDto>("expense", updateExpenseDto);
                }

                EditSuccessState.Success = true;
                // go to view to see the record
                _navigation.NavigateTo("/expense/list");
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