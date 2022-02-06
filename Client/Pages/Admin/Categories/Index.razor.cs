using Client.Services;
using Microsoft.AspNetCore.Components;

namespace Client.Pages.Admin.Categories
{

    public partial class Index : ComponentBase
    {
        [Inject] InMemoryDatabaseCache InMemoryDatabaseCache { get; set; }

        protected override async Task OnInitializedAsync()
        {
            InMemoryDatabaseCache.OnCategoriesDataChanged += StateHasChanged;

            if (InMemoryDatabaseCache.Categories == null)
            {
                await InMemoryDatabaseCache.GetCategoriesFromDatabaseAndCache();
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InMemoryDatabaseCache.OnCategoriesDataChanged -= StateHasChanged;
            }
        }
    }
}
