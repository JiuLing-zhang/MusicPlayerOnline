using System;
using MusicPlayerOnline.Player;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.ViewModels;
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

            this.Appearing += (sender, args) =>
            {
                _myModel.OnAppearing();
            };
        }
    }
}