using System;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Network;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultPage : ContentPage
    {
        private readonly SearchResultPageViewModel _myModel = new SearchResultPageViewModel();

        public Action<MusicDetail> SelectedFinished;
        public SearchResultPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }

        public void Search(string keyword)
        {
            _myModel.SearchKeyword = keyword;
        }

        private void SearchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _myModel.MusicSelectedResult = e.CurrentSelection[0] as SearchResultViewModel;
            Navigation.PopAsync();
        }
    }
}