using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnline.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogPage : ContentPage
    {
        private readonly LogPageViewModel _myModel = new LogPageViewModel();
        public LogPage()
        {
            InitializeComponent();
            BindingContext = _myModel;

            this.Appearing += (_, __) =>
            {
                _myModel.OnAppearing();
            };
        }
    }
}