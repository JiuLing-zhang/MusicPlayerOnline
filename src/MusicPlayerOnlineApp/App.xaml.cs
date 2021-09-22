using MusicPlayerOnlineApp.Services;
using System.IO;
using MusicPlayerOnline.Data;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            if (!Directory.Exists(GlobalArgs.AppDataPath))
            {
                Directory.CreateDirectory(GlobalArgs.AppDataPath);
            }
            DatabaseProvide.SetConnection(GlobalArgs.AppDbPath);
            DatabaseProvide.InitTable();

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
