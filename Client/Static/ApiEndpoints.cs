namespace Client.Static
{
    internal static class ApiEndpoints
    {
#if DEBUG
        internal const string ServerBaseUrl = "https://localhost:5003";
#else
        internal const string ServerBaseUrl = "https://appname.azurewebsites.net";
#endif

        internal readonly static string s_catetories = $"{ServerBaseUrl}/api/categories";
    }
}
