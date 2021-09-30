using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;

namespace MusicPlayerOnlineApp.Droid
{
    [Activity(Label = "在线音乐助手", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uiOpts = SystemUiFlags.LayoutStable | SystemUiFlags.LayoutFullscreen;
            //LayoutStable表示布局稳定，不随其他变动而变动
            //LayoutFullscreen表示把布局拓宽到全屏幕
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOpts;
            //把标题栏设置为透明色
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(0, 0, 0, 0));

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
        }
    }
}