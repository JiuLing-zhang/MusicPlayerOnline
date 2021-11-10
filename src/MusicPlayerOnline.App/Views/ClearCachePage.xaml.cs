using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClearCachePage : ContentPage
    {
        private readonly ClearCachePageViewModel _myModel = new ClearCachePageViewModel();
        public ClearCachePage()
        {
            InitializeComponent();
            BindingContext = _myModel;

            this.Appearing += (_, __) =>
            {
                _myModel.OnAppearing();
            };
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            _myModel.CalcSelectedSize();
        }
    }
}