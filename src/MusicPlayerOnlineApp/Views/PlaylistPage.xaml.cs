﻿using System;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistPage : ContentPage
    {
        private readonly PlaylistPageViewModel _myModel = new PlaylistPageViewModel();

        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = _myModel;

            this.Appearing += (_, __) =>
            {
                _myModel.OnAppearing();
            };
        }
        private void Entry_Completed(object sender, EventArgs e)
        {
            _myModel.Search();
        }
    }
}