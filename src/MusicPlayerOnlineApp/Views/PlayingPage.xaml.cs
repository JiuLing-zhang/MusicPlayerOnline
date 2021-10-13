﻿using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayingPage : ContentPage
    {
        private readonly PlayingPageViewModel _myModel = new PlayingPageViewModel();
        public PlayingPage()
        {
            InitializeComponent();
            BindingContext = _myModel;

            _myModel.ScrollLyric += (lyricItem) =>
            {
                ListViewLyrics.ScrollTo(lyricItem, ScrollToPosition.Center, true);
            };
            this.Appearing += (_, __) =>
            {
                _myModel.OnAppearing();
            };
        }
    }
}