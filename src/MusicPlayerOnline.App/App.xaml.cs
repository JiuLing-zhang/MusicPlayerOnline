﻿using System;
using System.IO;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Common;
using MusicPlayerOnline.App.Models;
using MusicPlayerOnline.App.Services;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Log;
using Xamarin.Forms;

namespace MusicPlayerOnline.App
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
                        DependencyService.Get<IToast>().Show("自动更新检查失败，服务器返回数据格式异常");
                        await Logger.WriteAsync(LogTypeEnum.错误, "自动更新检查失败，服务器返回数据格式异常");
                        return;
                    }

                    if (obj.Code != 0)
                    {
                        DependencyService.Get<IToast>().Show($"自动更新检查失败:{obj.Message}");
                        await Logger.WriteAsync(LogTypeEnum.错误, $"自动更新检查失败:{obj.Message}");
                        return;
                    }

                    var newVersion = new Version(obj.Data.VersionName);
                    var minVersion = new Version(obj.Data.MinVersionName);
                    var currentVersion = new Version(DependencyService.Get<IAppVersionInfo>().GetVersionName());
                    var result = JiuLing.CommonLibs.VersionUtils.CheckNeedUpdate(currentVersion, newVersion, minVersion);

                    if (result.IsNeedUpdate == false)
                    {
                        await Logger.WriteAsync(LogTypeEnum.消息, $"本地为最新版本，不需要更新");
                        return;
                    }

                    string title = $"发现新版本：{obj.Data.VersionName}，是否更新？{Environment.NewLine}当前版本：{DependencyService.Get<IAppVersionInfo>().GetVersionName()}";
                    var isOk = await App.Current.MainPage.DisplayAlert("提示", title, "是", "否");
                    if (isOk == false)
                    {
                        if (result.IsAllowUse == false)
                        {
                            await App.Current.MainPage.DisplayAlert("警告", "程序版本太低，无法使用", "确定");
                            await Logger.WriteAsync(LogTypeEnum.错误, $"程序版本太低，无法使用");
                            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                        }
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
