using System.IO;
using System.Linq;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Services;
using MusicPlayerOnlineApp.Views;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Log;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalMethods
    {
        private static readonly IConfigService MyConfigService = new ConfigService();
        private static readonly IMusicService MyMusicService = new MusicService();
        public static void ReadAppConfig()
        {
            GlobalArgs.AppConfig = new Config
            {
                General = MyConfigService.ReadGeneralConfig(),
                Search = MyConfigService.ReadPlatformConfig(),
                Play = MyConfigService.ReadPlayConfig(),
                Player = MyConfigService.ReadPlayerConfig()
            };
        }
        /// <summary>
        /// 保存通用配置
        /// </summary>
        public static async Task WriteGeneralConfig()
        {
            await MyConfigService.WriteGeneralConfigAsync(GlobalArgs.AppConfig.General);
        }
        /// <summary>
        /// 保存平台设置
        /// </summary>
        public static async Task WritePlatformConfig()
        {
            await MyConfigService.WritePlatformConfigAsync(GlobalArgs.AppConfig.Search);
        }
        /// <summary>
        /// 保存播放配置
        /// </summary>
        public static async Task WritePlayConfig()
        {
            await MyConfigService.WritePlayConfigAsync(GlobalArgs.AppConfig.Play);
        }
        /// <summary>
        /// 保存播放器配置
        /// </summary>
        public static async Task WritePlayerConfig()
        {
            await MyConfigService.WritePlayerConfigAsync(GlobalArgs.AppConfig.Player);
        }

        public static async Task PlayMusic(MusicDetail music)
        {
            await Logger.WriteAsync(LogTypeEnum.消息, $"准备播放歌曲：{music.Platform.GetDescription()},{music.Name}");
            string cachePath = Path.Combine(GlobalArgs.AppMusicCachePath, music.Id);
            if (!File.Exists(cachePath))
            {
                await Logger.WriteAsync(LogTypeEnum.消息, $"歌曲未缓存，准备缓存");
                var wifi = Plugin.Connectivity.Abstractions.ConnectionType.WiFi;
                var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
                if (!connectionTypes.Contains(wifi) && GlobalArgs.AppConfig.Play.IsWifiPlayOnly)
                {
                    await Logger.WriteAsync(LogTypeEnum.消息, $"仅在WIFI下允许播放，跳过");
                    DependencyService.Get<IToast>().Show("仅在WIFI下允许播放");
                    return;
                }

                await MyMusicService.CacheMusic(music, cachePath);
                music.CachePath = cachePath;
                await Logger.WriteAsync(LogTypeEnum.消息, $"缓存完成");
            }
            PlayerService.Instance().Play(music);
        }

        public static void ShowLoading()
        {
            DependencyService.Get<ILoadingPageService>().InitLoadingPage(new LoadingPage());
            DependencyService.Get<ILoadingPageService>().ShowLoadingPage();
        }

        public static void HideLoading()
        {
            DependencyService.Get<ILoadingPageService>().HideLoadingPage();
        }
    }
}
