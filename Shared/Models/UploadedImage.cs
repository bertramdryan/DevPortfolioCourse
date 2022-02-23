namespace Shared.Models
{
    public class UploadedImage
    {
        public string NewImageFileExtention { get; set; }
        //base64 is a string that reprents binary
        public string NewImageBase64Content { get; set; }
        public string OldImagePath { get; set; }
    }
}
