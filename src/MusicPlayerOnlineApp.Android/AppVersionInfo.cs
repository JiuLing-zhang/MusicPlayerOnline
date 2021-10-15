using System;
using Android.Content.PM;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionInfo))]
namespace MusicPlayerOnlineApp.Droid
{
    public class AppVersionInfo : IAppVersionInfo
    {
        PackageInfo _appInfo;
        public AppVersionInfo()
        {
            var context = Android.App.Application.Context;
            _appInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
        }
        public string GetVersionCode()
        {
            return _appInfo.VersionCode.ToString();
        }

        public string GetVersionName()
        {
            return _appInfo.VersionName;
        }
    }
}