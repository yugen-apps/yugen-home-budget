using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Yugen.HomeBudget.Shared.Models.Category;
using Yugen.HomeBudget.Shared.Models.Expense;

namespace Yugen.HomeBudget.Client.Pages.Category
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

        private bool _showPopup;

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
        /// Category entity.
        /// </summary>
        private CategoryDto? CategoryDto { get; set; }

        private string? NewSubCategoryTitle { get; set; }

        /// <summary>
        /// Start it up
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnInitializedAsync()
        {
            _busy = true;

            try
            {
                if (Id != null)
                {
                    await LoadAsync();
                }
                else
                {
                    CategoryDto = new CategoryDto(0, "");
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
            CategoryDto = null;

            //try
            //{
                CategoryDto = await _httpClient.GetFromJsonAsync<CategoryDto>($"category/{Id}");
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
            _navigation.NavigateTo("/category/list");
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
                    var createCategoryDto = new CreateCategoryDto(CategoryDto.Title, CategoryDto.SubCategoriesDto);
                    var response = await _httpClient.PutAsJsonAsync<CreateCategoryDto>($"category/{Id}", createCategoryDto);
                    //var c = await response.Content.ReadFromJsonAsync<CategoryDto>();
                }
                else
                {
                    var updateCategoryDto = new UpdateCategoryDto(CategoryDto.Id, CategoryDto.Title, CategoryDto.SubCategoriesDto);
                    var response = await _httpClient.PostAsJsonAsync<UpdateCategoryDto>("category", updateCategoryDto);
                }

                EditSuccessState.Success = true;
                // go to view to see the record
                _navigation.NavigateTo("/category/list");
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
            CategoryDto?.SubCategoriesDto.Remove(subCategoryDto);
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
            CategoryDto?.SubCategoriesDto.Add(new SubCategoryDto(0, NewSubCategoryTitle));
            ClosePopup();
        }
    }
}