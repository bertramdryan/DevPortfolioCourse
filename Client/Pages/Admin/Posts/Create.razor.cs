using System.Net;
using System.Net.Http.Json;
using Client.Services;
using Client.Static;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Models;

namespace Client.Pages.Admin.PostCreate;

public partial class Create : ComponentBase
{
    [Inject] HttpClient HttpClient { get; set; }
    [Inject] InMemoryDatabaseCache InMemoryDatabaseCache { get; set; }

    private PostDto _postToCreate = new PostDto() { ThumbnailImagePath = "uploads/placeholder.jpg"};
    private bool _attemptingToCreate = false;
    private bool _attemptToCrateFailed = false;
    private bool _createSuccessful = false;

    private async Task CreatePost()
    {
        _attemptingToCreate = true;

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync<PostDto>(ApiEndpoints.s_catetories, _postToCreate);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            Post addedPost = await response.Content.ReadFromJsonAsync<Post>();
            InMemoryDatabaseCache.Posts.Add(addedPost);

            _createSuccessful = true;
        }
        else
        {
            _attemptToCrateFailed = true;
        }        
    }

    private bool _attemptingUploadImage = false;
    private bool _attemptUploadFailed = false;
    private string _reasonImageUploadFailed = null;
    private async Task HandleUploadedImage(InputFileChangeEventArgs e)
    {
        _attemptingUploadImage = true;

        if(e.File.ContentType != "image/jpeg" && e.File.ContentType != "image/png")
        {
            _reasonImageUploadFailed = "Please only upload JPG, JPEG, or PNG images.";
            _attemptUploadFailed = true;
        }
        else if(e.File.Size >= 31457280) // 30mb
        {
            _reasonImageUploadFailed = "Please keep files under 30mb";
            _attemptUploadFailed = true;
        }
        else
        {
            IBrowserFile uploadedImageFile = e.File;

            byte[] imageAsByArray = new byte[uploadedImageFile.Size];

            // fills the content of image byte array
            await uploadedImageFile.OpenReadStream(31457280).ReadAsync(imageAsByArray);
            string byteString = Convert.ToBase64String(imageAsByArray);

            UploadedImage uploadedImage = new UploadedImage()
            {
                NewImageFileExtention = uploadedImageFile.Name.Substring(uploadedImageFile.Name.Length - 4),
                NewImageBase64Content = byteString,
                OldImagePath = string.Empty,
            };

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync<UploadedImage>(ApiEndpoints.s_imageUpload, uploadedImage);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                _postToCreate.ThumbnailImagePath = await response.Content.ReadAsStringAsync();
            } else
            {
                _reasonImageUploadFailed = $"Something when wrong when making a request to the server. Server Code: {response.StatusCode}. Server reason: {response.ReasonPhrase}";
                _attemptUploadFailed=true;
            }
        }

        _attemptingUploadImage = false;
        StateHasChanged();
    }
}