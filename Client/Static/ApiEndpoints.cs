namespace Client.Static
{
    internal static class ApiEndpoints
    {
#if DEBUG
        internal const string ServerBaseUrl = "https://localhost:5003";
#else
        internal const string ServerBaseUrl = "https://devportfoliobert.azurewebsites.net";
#endif

        internal readonly static string s_catetories = $"{ServerBaseUrl}/api/categories";
        internal readonly static string s_catetoriesWithPosts = $"{ServerBaseUrl}/api/categories/withposts";
        internal readonly static string s_posts = $"{ServerBaseUrl}/api/posts";
        internal readonly static string s_postsDto = $"{ServerBaseUrl}/api/categories/posts/dto";
        internal readonly static string s_imageUpload = $"{ServerBaseUrl}/api/ImageUpload";
        internal readonly static string s_signIn = $"{ServerBaseUrl}/api/signin";
    }
}
