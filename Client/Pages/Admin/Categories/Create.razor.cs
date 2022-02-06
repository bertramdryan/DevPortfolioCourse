using System.Net;
using System.Net.Http.Json;
using Client.Services;
using Client.Static;
using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Client.Pages.Admin.Categories;

public partial class Create : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] InMemoryDatabaseCache InMemoryDatabaseCache { get; set; }

    private Category _categoryToCreate = new Category() { ThumbnailImagePath = "uploads/placeholder.jpg", Posts = new List<Post>() };
    private bool _attemptingToCreate = false;
    private bool _attemptToCrateFailed = false;
    private bool _createSuccessful = false;

    private async Task CreateCategory()
    {
        _attemptingToCreate = true;

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync<Category>(ApiEndpoints.s_catetories, _categoryToCreate);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            Category addedCategory = await response.Content.ReadFromJsonAsync<Category>();
            InMemoryDatabaseCache.Categories.Add(addedCategory);

            _createSuccessful = true;
        }
        else
        {
            _attemptToCrateFailed = true;
        }

        _attemptingToCreate = false;
        
    }
}