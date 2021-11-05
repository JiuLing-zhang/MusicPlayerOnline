using System;
using MusicPlayerOnlineApp.Services;
using System.IO;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Log;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Models;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DatabaseProvide.SetConnection(GlobalArgs.AppDbFileName);

            Logger.Write(LogTypeEnum.消息, $"程序启动");
            Logger.Write(LogTypeEnum.消息, $"准备初始化配置");
            if (!Directory.Exists(GlobalArgs.AppDataPath))
            {
                Directory.CreateDirectory(GlobalArgs.AppDataPath);
            }
            if (!Directory.Exists(GlobalArgs.AppMusicCachePath))
            {
                Directory.CreateDirectory(GlobalArgs.AppMusicCachePath);
            }

            GlobalMethods.ReadAppConfig();

            Logger.Write(LogTypeEnum.消息, $"初始化配置完成");

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

            CheckUpdate();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        private async void CheckUpdate()
        {
            if (GlobalArgs.AppConfig.General.IsAutoCheckUpdate)
            {
                await Logger.WriteAsync(LogTypeEnum.消息, $"准备检查自动更新");
                try
                {
                    var httpClient = new JiuLing.CommonLibs.Net.HttpClientHelper();
                    string httpResult = await httpClient.GetReadString(GlobalArgs.UrlAppInfo);
                    if (httpResult.IsEmpty())
                    {
                        DependencyService.Get<IToast>().Show("自动更新失败，服务器状态异常");
                        await Logger.WriteAsync(LogTypeEnum.错误, "自动更新检查失败，服务器状态异常");
                        return;
                    }
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JiuLing.CommonLibs.Model.JsonResult<AppInfo>>(httpResult);
                    if (obj == null)
                    {
                        DependencyService.Get<IToast>().Show("更新失败，服务器返回数据格式异常");
                        await Logger.WriteAsync(LogTypeEnum.错误, "更新失败，服务器返回数据格式异常");
                        return;
                    }

                    if (obj.Code != 0)
                    {
                        DependencyService.Get<IToast>().Show($"更新失败:{obj.Message}");
                        await Logger.WriteAsync(LogTypeEnum.错误, $"更新失败:{obj.Message}");
                        return;
                    }

                    if (DependencyService.Get<IAppVersionInfo>().GetVersionCode() >= obj.Data.VersionCode)
                    {
                        await Logger.WriteAsync(LogTypeEnum.消息, $"本次不需要更新");
                        return;
                    }

                    string title = $"发现新版本：{obj.Data.VersionName}，是否更新？{Environment.NewLine}当前版本：{DependencyService.Get<IAppVersionInfo>().GetVersionName()}";
                    var isOk = await App.Current.MainPage.DisplayAlert("提示", title, "是", "否");
                    if (isOk == false)
                    {
                        return;
                    }
                    DependencyService.Get<IAppVersionInfo>().Update(obj.Data.DownloadUrl);
                }
                catch (Exception ex)
                {
                    await Logger.WriteAsync(LogTypeEnum.错误, $"自动更新失败，{ex.Message}.{ex.StackTrace}");
                }
            }
        }
    }
}
