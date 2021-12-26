using Microsoft.AspNetCore.Components;

namespace Client.Components.Public.Shared
{
    public partial class Navbar
    {

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected const string HomePageUri = "/";
    protected const string SkillsPageUri = "/skills";
    protected const string PortfolioPageUri = "/portfolio";
    protected const string BlogPageUri = "/blog";
    protected const string AboutPageUri = "/about";
    protected const string ContactPageUri = "/contact";

    protected bool IsPageActive(string pageToCheckUri)
    {
        // skip(3) to remove https://domainname/
        string[] currentUrisplitBetweenSlashes = NavigationManager.Uri.Split('/').Skip(3).ToArray();

        if (pageToCheckUri == HomePageUri)
        {
            if (currentUrisplitBetweenSlashes.Last().Length == 0)
            {
                return true;
            }
        }
        else
        {
            string[] pageToCheckUriSPlitBetweenSlashes = pageToCheckUri.Split('/').Skip(1).ToArray();

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
    