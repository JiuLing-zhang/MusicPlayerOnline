using System.IO;
using System.Linq;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Services;
using MusicPlayerOnlineApp.Views;
using System.Threading.Tasks;
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

        public static void PlayMusic(MusicDetail music)
        {
            string cachePath = Path.Combine(GlobalArgs.AppMusicCachePath, music.Id);
            if (!File.Exists(cachePath))
            {
                var wifi = Plugin.Connectivity.Abstractions.ConnectionType.WiFi;
                var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
                if (!connectionTypes.Contains(wifi) && GlobalArgs.AppConfig.Play.IsWifiPlayOnly)
                {
                    DependencyService.Get<IToast>().Show("仅在WIFI下允许播放");
                    return;
                }

                MyMusicService.CacheMusic(music, cachePath);
                music.CachePath = cachePath;
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
