using Microsoft.AspNetCore.Components;
using Yugen.HomeBudget.Shared.Models.Category;

namespace Yugen.HomeBudget.Client.Controls
{
    public partial class SubCategoryControl
    {
        [Parameter]
        public SubCategoryDto SubCategoryDto { get; set; }

        [Parameter]
        public EventCallback<SubCategoryDto> DeleteCallback { get; set; }

        /// <summary>
        /// Confirm the delete.
        /// </summary>
        private bool DeleteConfirmation { get; set; }

        /// <summary>
        /// Avoid concurrent requests
        /// </summary>
        private bool _busy;

        /// <summary>
        /// Set delete to true.
        /// </summary>
        private void DeleteRequestAsync()
        {
            DeleteConfirmation = true;
        }

        /// <summary>
        /// Called based on confirmation.
        /// </summary>
        /// <param name="confirmed"><c>True</c> when confirmed</param>
        /// <returns>A <see cref="Task"/>.</returns>
        private void ConfirmAsync(bool confirmed)
        {
            if (confirmed)
            {
                DeleteCallback.InvokeAsync(SubCategoryDto);
            }

            DeleteConfirmation = false;
        }
    }
}
