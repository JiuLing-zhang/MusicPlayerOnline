using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Services;
using MusicPlayerOnlineApp.Views;
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

            if (GlobalArgs.AppConfig.General == null)
            {
                await MyConfigService.WriteGeneralConfig(new GeneralConfig());
            }
            if (GlobalArgs.AppConfig.Play == null)
            {
                await MyConfigService.WritePlayConfig(new PlayConfig());
            }
            if (GlobalArgs.AppConfig.Player == null)
            {
                await MyConfigService.WritePlayerConfig(new PlayerConfig());
            }
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
