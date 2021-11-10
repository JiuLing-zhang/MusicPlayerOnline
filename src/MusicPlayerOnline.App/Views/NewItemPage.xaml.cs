using System;
using System.Collections.Generic;
using System.ComponentModel;
using MusicPlayerOnline.App.Models;
using MusicPlayerOnline.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnline.App.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}