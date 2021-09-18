using System;
using System.Linq;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnline.Network;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistPage : ContentPage
    {
        private readonly PlaylistPageViewModel _myModel = new PlaylistPageViewModel();
        private readonly SearchResultPage _searchResultPage = new SearchResultPage();
        private readonly MusicNetPlatform _musicNetPlatform = new MusicNetPlatform();
        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = _myModel;

            this.Appearing += (sender, args) =>
            {
                OnPageAppearing();
            };
        }

        private void TxtKeywordEntry_Completed(object sender, EventArgs e)
        {
            if (_myModel.SearchKeyword.IsEmpty())
            {
                return;
            }
            Navigation.PushAsync(_searchResultPage);
            _searchResultPage.Search(_myModel.SearchKeyword);
        }

        private void OnPageAppearing()
        {
            _myModel.SearchKeyword = "";
            if (_searchResultPage.SelectedMusicDetail == null)
            {
                return;
            }
            var music = _searchResultPage.SelectedMusicDetail;
            Task.Run(() =>
            {
                if (_myModel.Playlist.Any(x => x.Id == music.Id))
                {
                    //已在播放列表包含，跳过
                    return;
                }

                _myModel.Playlist.Add(new PlaylistViewModel()
                {
                    Id = music.Id,
                    Name = music.Name,
                    Artist = music.Artist,
                    ImageUrl = music.ImageUrl,
                    SourceData = music
                });
                _myModel.IsMusicsEmpty = !_myModel.Playlist.Any();
                PlayMusic(music);
            });
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlaylistViewModel selectedMusic = e.CurrentSelection[0] as PlaylistViewModel;
            PlayMusic(selectedMusic.SourceData);
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void PlayMusic(MusicDetail music)
        {
            Task.Run(() =>
            {
                if (music.Platform == PlatformEnum.Netease)
                {
                    music = _musicNetPlatform.UpdateMusicDetail(music).Result;
                }
                foreach (var item in _myModel.Playlist)
                {
                    if (item.IsPlaying == true)
                    {
                        item.IsPlaying = false;
                        break;
                    }
                }

                _myModel.CurrentMusic = music;
            });
        }
    }
}