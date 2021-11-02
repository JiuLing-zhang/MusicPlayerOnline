using MusicPlayerOnlineApp.Services;
using System.IO;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Log;
using MusicPlayerOnlineApp.Common;
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
    }
}
