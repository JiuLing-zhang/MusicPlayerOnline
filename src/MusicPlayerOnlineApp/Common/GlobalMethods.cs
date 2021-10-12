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
                GlobalArgs.AppConfig.General = new GeneralConfig();
                await MyConfigService.WriteGeneralConfig(GlobalArgs.AppConfig.General);
            }
            if (GlobalArgs.AppConfig.Play == null)
            {
                GlobalArgs.AppConfig.Play = new PlayConfig();
                await MyConfigService.WritePlayConfig(GlobalArgs.AppConfig.Play);
            }
            if (GlobalArgs.AppConfig.Player == null)
            {
                GlobalArgs.AppConfig.Player = new PlayerConfig();
                await MyConfigService.WritePlayerConfig(GlobalArgs.AppConfig.Player);
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
