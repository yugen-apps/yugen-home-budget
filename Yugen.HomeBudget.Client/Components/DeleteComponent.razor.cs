using Microsoft.AspNetCore.Components;

namespace Yugen.HomeBudget.Client.Components
{
    public partial class DeleteComponent
    {
        /// <summary>
        /// Avoid concurrent requests
        /// </summary>
        private bool _busy;

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public EventCallback<int> DeleteCallback { get; set; }

        private bool DeleteConfirmation { get; set; }

        private void DeleteRequestAsync()
        {
            DeleteConfirmation = true;
        }

        private void ConfirmAsync(bool confirmed)
        {
            _busy = true;

            if (confirmed)
            {
                DeleteCallback.InvokeAsync(Id);
            }

            DeleteConfirmation = false;

            _busy = false;
        }
    }
}