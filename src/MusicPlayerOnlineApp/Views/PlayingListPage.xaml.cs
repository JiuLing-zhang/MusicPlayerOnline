using System;
using System.Linq;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnline.Network;
using MusicPlayerOnline.Player;
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
        private readonly IAudio audio;
        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
            this.Appearing += (sender, args) =>
            {
                OnPageAppearing();
            };
            LoadPlaylist();

            audio = DependencyService.Get<IAudio>();
            audio.MediaBegin += Audio_MediaBegin;
            audio.MediaEnded += Audio_MediaEnded;
            audio.MediaFailed += Audio_MediaFailed;
        }
        private void Audio_MediaBegin()
        {
            _myModel.IsPlaying = true;
        }

        private void Audio_MediaEnded()
        {
            _myModel.IsPlaying = false;
        }
        private void Audio_MediaFailed()
        {
            throw new NotImplementedException();
        }


        private async void LoadPlaylist()
        {
            await Task.Run(() =>
            {
                var playlist = DatabaseProvide.Database.Table<Playlist>().ToListAsync().Result;
                foreach (var m in playlist)
                {
                    _myModel.Playlist.Add(new PlaylistViewModel()
                    {
                        MusicDetailId = m.MusicDetailId,
                        Name = m.Name,
                        Artist = m.Artist
                    });
                }
            });
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

        private async void OnPageAppearing()
        {
            _myModel.SearchKeyword = "";
            if (_searchResultPage.SelectedMusicDetail == null)
            {
                return;
            }
            var music = _searchResultPage.SelectedMusicDetail;
            await Task.Run(() =>
            {
                if (DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == music.Id).CountAsync().Result == 0)
                {
                    DatabaseProvide.Database.InsertAsync(music);
                }

                if (DatabaseProvide.Database.Table<Playlist>().Where(x => x.MusicDetailId == music.Id).CountAsync().Result == 0)
                {
                    DatabaseProvide.Database.InsertAsync(new Playlist() { MusicDetailId = music.Id, Name = music.Name, Artist = music.Artist });
                }

                if (_myModel.Playlist.All(x => x.MusicDetailId != music.Id))
                {
                    _myModel.Playlist.Add(new PlaylistViewModel()
                    {
                        MusicDetailId = music.Id,
                        Name = music.Name,
                        Artist = music.Artist
                    });
                }
                PlayMusic(music.Id);
            });
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlaylistViewModel selectedMusic = e.CurrentSelection[0] as PlaylistViewModel;
            PlayMusic(selectedMusic.MusicDetailId);
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void PlayMusic(string id)
        {
            Task.Run(() =>
            {
                var music = DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == id).FirstOrDefaultAsync().Result;
                if (music == null)
                {
                    DisplayAlert("播放失败", "歌曲信息在本地未找到。。。", "确定");
                    return;
                }

                if (music.Platform == PlatformEnum.Netease)
                {
                    music = _musicNetPlatform.UpdateMusicDetail(music).Result;
                }
                _myModel.CurrentMusic = music;
                DependencyService.Get<IAudio>().Play(music.PlayUrl);
            });
        }
    }
}