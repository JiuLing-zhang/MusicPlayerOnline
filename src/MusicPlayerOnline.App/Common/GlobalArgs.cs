﻿using System;
using System.IO;
using System.Reflection;
using MusicPlayerOnline.App.Models;
using MusicPlayerOnline.Model.Model;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.Common
{
    public class GlobalArgs
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        private static AppSettings _myAppSettings;
        public static AppSettings MyAppSettings
        {
            get
            {
                if (_myAppSettings == null)
                {
                    LoadAppSettings();
                }
                return _myAppSettings;
            }
        }
        public static string AppDbFileName = Path.Combine(AppDataPath, MyAppSettings.Database);
        public static string AppMusicCachePath = Path.Combine(AppDataPath, MyAppSettings.FileCachePath);
        public static Config AppConfig;

        /// <summary>
        /// 日志接口地址
        /// </summary>
        public static string UrlLog = $"{GlobalArgs.MyAppSettings.ApiAddress}/log";
        /// <summary>
        /// 最新的程序信息
        /// </summary>
        public static string UrlAppInfo => GetUrlAppInfo();
        private static string GetUrlAppInfo()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return $"{GlobalArgs.MyAppSettings.ApiAddress}/app/MusicPlayerOnline/android";
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                return $"{GlobalArgs.MyAppSettings.ApiAddress}/app/MusicPlayerOnline/ios";
            }

            throw new Exception("不支持的平台");
        }
        private static void LoadAppSettings()
        {
            var stream = Assembly.GetAssembly(typeof(AppSettings)).GetManifestResourceStream("MusicPlayerOnline.App.config.json");
            if (stream == null)
            {
                throw new Exception("本地配置文件加载失败");
            }

            using (var sr = new StreamReader(stream))
            {
                string json = sr.ReadToEnd();
                _myAppSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettings>(json);
            }
        }
    }

    /// <summary>
    /// 内部订阅时使用的关键字
    /// </summary>
    public class SubscribeKey
    {
        /// <summary>
        /// 更新播放列表
        /// </summary>
        public const string UpdatePlaylist = "UpdatePlaylist";
    }
}
