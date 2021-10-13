using MusicPlayerOnlineApp.ViewModels;
using MusicPlayerOnlineApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            Routing.RegisterRoute(nameof(SearchResultPage), typeof(SearchResultPage));
            Routing.RegisterRoute(nameof(AddToMyFavoritePage), typeof(AddToMyFavoritePage));
            Routing.RegisterRoute(nameof(AddMyFavoritePage), typeof(AddMyFavoritePage));
            Routing.RegisterRoute(nameof(EditMyFavoritePage), typeof(EditMyFavoritePage));
            Routing.RegisterRoute(nameof(MyFavoriteDetailPage), typeof(MyFavoriteDetailPage));
            Routing.RegisterRoute(nameof(PlayingPage), typeof(PlayingPage));
        }
    }
}
