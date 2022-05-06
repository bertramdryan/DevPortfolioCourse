using Client.Services;
using Client.Static;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net;

namespace Client.Pages.Admin.PostIndex
{

    public partial class Index : ComponentBase
    {
        [Inject] InMemoryDatabaseCache InMemoryDatabaseCache { get; set; }

        protected override async Task OnInitializedAsync()
        {
            InMemoryDatabaseCache.OnCategoriesDataChanged += StateHasChanged;

            if (InMemoryDatabaseCache.Categories == null)
            {
                await InMemoryDatabaseCache.GetCategoriesFromDatabaseAndCache(true);
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
        private bool _attemptingToDeleteAPost = false;
        private bool _successfullyDeletedAPost = false;
        private bool _unsuccesssfullyTriedToDeletePost = false;
        private string _reasonUnsuccessfullyTriredToDeletePost = null;

        private async void DeletePost(Post postToDelete)
        {
            _attemptingToDeleteAPost = true;
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{ApiEndpoints.s_posts}/{postToDelete.PostId}");

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                if (InMemoryDatabaseCache.Posts.Remove(postToDelete))
                {
                    _successfullyDeletedAPost = true;
                }
                else
                {
                    _reasonUnsuccessfullyTriredToDeletePost = "An unexpected error has occurred. Please try again and if the issue presists please contact administrator";
                    _unsuccesssfullyTriedToDeletePost = true;
                }
            }
            else
            {
                _reasonUnsuccessfullyTriredToDeletePost = $"The api didn't return an HttpStatusCode.NoContent. Instead the API returned status code {response.StatusCode} and gave the following reason for failure: {response.ReasonPhrase}.";
                _unsuccesssfullyTriedToDeletePost = true;
            }
            _attemptingToDeleteAPost = false;
            StateHasChanged();

        }
    }
}
