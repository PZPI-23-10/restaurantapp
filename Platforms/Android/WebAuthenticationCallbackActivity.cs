using Android.App;
using Android.Content;
using Android.Content.PM;
using Microsoft.Maui.Authentication;

namespace RestoranApp
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(
         new[] { Intent.ActionView },
         Categories = new[] {
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
         },
         DataScheme = "com.companyname.restoranapp",
         DataPath = "/auth")]
    public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    {
    }
}
