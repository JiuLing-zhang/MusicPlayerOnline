using System;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayingListPage : ContentPage
    {
        private readonly SearchResultPage _searchResultPage = new SearchResultPage();
        public PlayingListPage()
        {
            InitializeComponent();
            this.Appearing += (sender, args) =>
                { Play(); };
        }

        private void TxtKeywordEntry_Completed(object sender, EventArgs e)
        {
            string keyword = TxtKeyword.Text;
            if (keyword.IsEmpty())
            {
                return;
            }
            Navigation.PushAsync(_searchResultPage);
            _searchResultPage.Search(keyword);
        }

        private void Play()
        {
            if (_searchResultPage.SelectedMusicDetail == null)
            {
                TxtKeyword.Text = "";
                return;
            }
            var musicDetail = _searchResultPage.SelectedMusicDetail;
            Task.Run(() =>
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(musicDetail.PlayUrl);
            });
        }
    }
}