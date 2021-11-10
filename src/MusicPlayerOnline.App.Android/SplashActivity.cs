using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;

namespace MusicPlayerOnlineApp.Droid
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            StartActivity(typeof(MainActivity));
        }
    }
}