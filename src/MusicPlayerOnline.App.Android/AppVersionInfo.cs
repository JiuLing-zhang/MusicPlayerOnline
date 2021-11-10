using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.Content;
using Java.Net;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionInfo))]
namespace MusicPlayerOnline.App.Droid
{
    public class AppVersionInfo : IAppVersionInfo
    {
        readonly PackageInfo _appInfo;
        public AppVersionInfo()
        {
            var context = Android.App.Application.Context;
            _appInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
        }
        public int GetVersionCode()
        {
            return _appInfo.VersionCode;
        }

        public string GetVersionName()
        {
            return _appInfo.VersionName;
        }

        public void Update(string downloadUrl)
        {
            string downloadPath = Android.OS.Environment.DirectoryDownloads;
            string absolutePath = Android.App.Application.Context.GetExternalFilesDir(downloadPath).AbsolutePath;
            var filePath = Path.Combine(absolutePath, "MusicPlayerOnline.App.apk");
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            if (File.Exists(filePath))
            {
                InstallApk(filePath);
                return;
                File.Delete(filePath);
            }

            Task.Run(() =>
            {
                URL url = new URL(downloadUrl);
                HttpURLConnection conn = (HttpURLConnection)url.OpenConnection();
                conn.Connect();
                Stream downloadStream = conn.InputStream;
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    byte[] buf = new byte[512];
                    do
                    {
                        int numread = downloadStream.Read(buf, 0, 512);
                        if (numread <= 0)
                        {
                            break;
                        }
                        fs.Write(buf, 0, numread);
                    } while (true);
                }
                InstallApk(filePath);
            });

        }
        private void InstallApk(string filePath)
        {
            var context = Forms.Context;
            if (context == null)
                return;
            // 通过Intent安装APK文件
            Intent intent = new Intent(Intent.ActionView);

            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            Android.Net.Uri contentUri = FileProvider.GetUriForFile(context, "com.jiuling.MusicPlayerOnline.App.fileprovider", new Java.IO.File(filePath));
            intent.SetDataAndType(contentUri, "application/vnd.android.package-archive");
            context.StartActivity(intent);
        }
    }
}