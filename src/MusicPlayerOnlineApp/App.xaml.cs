﻿using MusicPlayerOnlineApp.Services;
using System.IO;
using MusicPlayerOnline.Data;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;
using IAudio = MusicPlayerOnlineApp.AppInterface.IAudio;

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
            if (!Directory.Exists(GlobalArgs.AppMusicCachePath))
            {
                Directory.CreateDirectory(GlobalArgs.AppMusicCachePath);
            }
            DatabaseProvide.SetConnection(GlobalArgs.AppDbFileName);

            GlobalMethods.ReadAppConfig();

            DependencyService.Register<MockDataStore>();
            Common.GlobalArgs.Audio = DependencyService.Get<IAudio>();
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
