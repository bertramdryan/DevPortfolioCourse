using System;
using Microsoft.AspNetCore.Components;

namespace Client.Components.Admin.Sidebar
{
    public partial class Sidebar
    {

        [Inject] NavigationManager NavigationManager { get; set; }

        private static string AdminIndexPageUri = "/admin";

        private static string s_adminCategoriesPageUri = $"{AdminIndexPageUri}/categories";
        private static string s_adminPostsPageUri = $"{AdminIndexPageUri}/posts";


        protected bool IsPageActive(string pageToCheckUri)
        {
            // skip(3) to remove https://domainname/
            string[] currentUrisplitBetweenSlashes = NavigationManager.Uri.Split('/').Skip(2).ToArray();

            if (pageToCheckUri == AdminIndexPageUri)
            {
                if (currentUrisplitBetweenSlashes.Last() == "admin")
                {
                    return true;
                }
            }
            else
            {
                // skip(2) remove the starting slash and admin
                string[] pageToCheckUriSPlitBetweenSlashes = pageToCheckUri.Split('/').Skip(2).ToArray();

                foreach (string currentUriPart in currentUrisplitBetweenSlashes)
                {
                    foreach (string pageToCheckUriPart in pageToCheckUriSPlitBetweenSlashes)
                    {
                        if (pageToCheckUriPart == currentUriPart)
                        {
                            return true;
                        }
                    }
                }
            }
            // if thie code gets here then the pageToCheckUri is not the active page. So return false.
            return false;
        }
    }
}


