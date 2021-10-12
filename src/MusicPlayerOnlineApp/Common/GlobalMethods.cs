using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Services;
using MusicPlayerOnlineApp.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalMethods
    {
        private static readonly IConfigService MyConfigService = new ConfigService();
        public static async void ReadAppConfig()
        {
            GlobalArgs.AppConfig = new Config
            {
                General = await MyConfigService.ReadGeneralConfig(),
                Play = await MyConfigService.ReadPlayConfig(),
                Player = await MyConfigService.ReadPlayerConfig()
            };
        }
        /// <summary>
        /// 保存通用配置
        /// </summary>
        public static async Task WriteGeneralConfig()
        {
            await MyConfigService.WriteGeneralConfig(GlobalArgs.AppConfig.General);
        }
        /// <summary>
        /// 保存播放配置
        /// </summary>
        public static async Task WritePlayConfig()
        {
            await MyConfigService.WritePlayConfig(GlobalArgs.AppConfig.Play);
        }
        /// <summary>
        /// 保存播放器配置
        /// </summary>
        public static async Task WritePlayerConfig()
        {
            await MyConfigService.WritePlayerConfig(GlobalArgs.AppConfig.Player);
        }

        public static void PlayMusic(MusicDetail music)
        {
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
