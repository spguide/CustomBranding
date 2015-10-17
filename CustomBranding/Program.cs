using Microsoft.SharePoint.Client;
using System;

namespace CustomBranding
{
  class Program
  {
    static void Main(string[] args)
    {
      Uri siteUri = new Uri("https://your-domain.sharepoint.com");
    string realm = TokenHelper.GetRealmFromTargetUrl(siteUri);
 
    string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal,
                                                            siteUri.Authority, realm).AccessToken;
 
    using (var clientContext = TokenHelper.GetClientContextWithAccessToken(siteUri.ToString(), accessToken))
    {
      Web web = clientContext.Web;
      web.UploadThemeFile("../../custom.spcolor");
      web.UploadThemeFile("../../Background.png");

      clientContext.Load(web, w => w.AllProperties, w => w.ServerRelativeUrl);
      clientContext.ExecuteQuery();
      // Let's first upload the custom theme to host web
      web.CreateComposedLookByName("customTheme",
                      web.ServerRelativeUrl + "/_catalogs/theme/15/custom.spcolor",
                      null,
                      clientContext.Web.ServerRelativeUrl + "/_catalogs/theme/15/Background.png",
                      string.Empty);
      clientContext.ExecuteQuery();
      // Setting the custom theme to host web
      web.SetComposedLookByUrl("customTheme");
      //web.SetComposedLookByUrl("Office");
    }
 
    Console.WriteLine("...");
    Console.ReadLine();
    }
  }
}
