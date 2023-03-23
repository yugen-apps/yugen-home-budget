using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Client.Services;
using Yugen.HomeBudget.Shared.Contants;
using Yugen.HomeBudget.Shared.Models.Authentication;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Pages.Category
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

        private bool _showPopup;

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
        /// Category entity.
        /// </summary>
        private Yugen.HomeBudget.Client.Models.Category? Category { get; set; }

        private string? NewSubCategoryTitle { get; set; }

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
                if (Id != null)
                {
                    await LoadAsync();
                }
                else
                {
                    Category = new(0, "");
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
            Category = null;

            //try
            //{
            var categoryDto = await _httpClient.GetFromJsonAsync<ResponseCategoryDto>($"{EndpointConstants.Category}/{Id}");
            Category = new(categoryDto.Id, categoryDto.Title, categoryDto.SubCategoriesDto);
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
            _navigationManager.NavigateTo(PageConstants.CategoryUrl);
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
                    var createCategoryDto = new CreateCategoryDto(Category.Title, Category.SubCategoriesDto, _currentUser?.Id, _currentUser?.Id);
                    var response = await _httpClient.PutAsJsonAsync<CreateCategoryDto>($"{EndpointConstants.Category}/{Id}", createCategoryDto);
                    //var c = await response.Content.ReadFromJsonAsync<CategoryDto>();
                }
                else
                {
                    var updateCategoryDto = new UpdateCategoryDto(Category.Id, Category.Title, Category.SubCategoriesDto, _currentUser?.Id, _currentUser?.Id);
                    var response = await _httpClient.PostAsJsonAsync<UpdateCategoryDto>($"{EndpointConstants.Category}", updateCategoryDto);
                }

                EditSuccessState.Success = true;
                // go to view to see the record
                _navigationManager.NavigateTo(PageConstants.CategoryUrl);
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

        private void DeleteSubCategoryAsync(SubCategoryDto subCategoryDto)
        {
            Category?.SubCategoriesDto.Remove(subCategoryDto);
        }

        private void ShowAddPopup()
        {
            NewSubCategoryTitle = string.Empty;
            _showPopup = true;
        }

        private void ClosePopup()
        {
            _showPopup = false;
        }

        private void AddSubCategory()
        {
            Category?.SubCategoriesDto.Add(new SubCategoryDto(0, NewSubCategoryTitle));
            ClosePopup();
        }
    }
}