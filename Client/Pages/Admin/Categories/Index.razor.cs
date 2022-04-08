using Client.Services;
using Client.Static;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net;

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

        [Inject] HttpClient HttpClient { get; set; }
        private bool _attemptingToDeleteACategory = false;
        private bool _successfullyDeletedACategory = false;
        private bool _unsuccesssfullyTriedToDeleteCategory = false;
        private string _reasonUnsuccessfullyTriredToDeleteCategory = null;

        private async void DeleteCategory(Category categoryToDelete)
        {
            if (categoryToDelete.Posts != null && categoryToDelete.Posts.Count != 0)
            {
                string postTitlesThatMustbeDeletedFirst = string.Empty;

                for (int i = 0; i < categoryToDelete.Posts.Count; i++)
                {
                    if (i == categoryToDelete.Posts.Count - 1)
                    {
                        postTitlesThatMustbeDeletedFirst += $"\"{categoryToDelete.Posts[i].Title}\"";
                    }
                    else if (i == categoryToDelete.Posts.Count - 2)
                    {
                        postTitlesThatMustbeDeletedFirst += $"\"{categoryToDelete.Posts[i].Title}\" and";
                    }
                    else
                    {
                        postTitlesThatMustbeDeletedFirst += $"\"{categoryToDelete.Posts[i].Title}\", ";
                    }
                }

                _reasonUnsuccessfullyTriredToDeleteCategory = $"The category still has posts in it. Please delete the posts posts titles {postTitlesThatMustbeDeletedFirst} first";
                _unsuccesssfullyTriedToDeleteCategory = true;
            } else
            {
                _attemptingToDeleteACategory = true;
                HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiEndpoints.s_catetories}/{categoryToDelete.CategoryId}");

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    if (InMemoryDatabaseCache.Categories.Remove(categoryToDelete))
                    {
                        _successfullyDeletedACategory = true;
                    } 
                    else
                    {
                        _reasonUnsuccessfullyTriredToDeleteCategory = "An unexpected error has occurred. Please try again and if the issue presists please contact administrator";
                        _unsuccesssfullyTriedToDeleteCategory = true;
                    }
                } 
                else
                {
                    _reasonUnsuccessfullyTriredToDeleteCategory = $"The api didn't return an HttpStatusCode.NoContent. Instead the API returned status code {response.StatusCode} and gave the following reason for failure: {response.ReasonPhrase}.";
                    _unsuccesssfullyTriedToDeleteCategory= true;
                }
            }
            _attemptingToDeleteACategory = false;
            StateHasChanged();
        }
    }
}
