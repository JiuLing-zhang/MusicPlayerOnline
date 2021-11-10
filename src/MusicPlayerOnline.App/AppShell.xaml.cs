using MusicPlayerOnline.App.ViewModels;
using System;
using System.Collections.Generic;
using MusicPlayerOnline.App.Views;
using MusicPlayerOnline.Log;
using Xamarin.Forms;

namespace MusicPlayerOnline.App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Logger.Write(LogTypeEnum.消息, $"准备注册路由");
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            Routing.RegisterRoute(nameof(SearchResultPage), typeof(SearchResultPage));
            Routing.RegisterRoute(nameof(AddToMyFavoritePage), typeof(AddToMyFavoritePage));
            Routing.RegisterRoute(nameof(AddMyFavoritePage), typeof(AddMyFavoritePage));
            Routing.RegisterRoute(nameof(EditMyFavoritePage), typeof(EditMyFavoritePage));
            Routing.RegisterRoute(nameof(MyFavoriteDetailPage), typeof(MyFavoriteDetailPage));
            Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
            Routing.RegisterRoute(nameof(ClearCachePage), typeof(ClearCachePage));
            Routing.RegisterRoute(nameof(LogPage), typeof(LogPage));
            Logger.Write(LogTypeEnum.消息, $"路由注册完成");
        }
    }
}
