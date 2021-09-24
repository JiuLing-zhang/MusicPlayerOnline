using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnline.Network;
using MusicPlayerOnline.Player;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
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
        private readonly AddToMyFavoritePage _addToMyFavoritePage = new AddToMyFavoritePage();

        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
            this.Appearing += (sender, args) =>
            {
                OnPageAppearing();
            };
            LoadPlaylist();

            Common.GlobalArgs.Audio = DependencyService.Get<IAudio>();
            Common.GlobalArgs.Audio.MediaBegin += Audio_MediaBegin;
            Common.GlobalArgs.Audio.MediaEnded += Audio_MediaEnded;
            Common.GlobalArgs.Audio.MediaFailed += Audio_MediaFailed;

            _myModel.CurrentMusic = new MusicDetail()
            {
                Name = "未开始播放",
                ImageUrl = "music_record"
            };
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
                    _myModel.Playlist.Add(new MusicDetailViewModel()
                    {
                        Id = m.MusicDetailId,
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

                if (_myModel.Playlist.All(x => x.Id != music.Id))
                {
                    _myModel.Playlist.Add(new MusicDetailViewModel()
                    {
                        Id = music.Id,
                        Name = music.Name,
                        Artist = music.Artist
                    });
                }
                PlayMusic(music.Id);
            });
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MusicDetailViewModel selectedMusic = e.CurrentSelection[0] as MusicDetailViewModel;
            PlayMusic(selectedMusic.Id);
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void PlayMusic(string id)
        {
            Task.Run(() =>
            {
                try
                {
                    var music = DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == id).FirstOrDefaultAsync().Result;
                    if (music == null)
                    { 
                        DependencyService.Get<IToast>().Show("未找到歌曲信息");
                        return;
                    }

                    string musicCache = Path.Combine(Common.GlobalArgs.AppMusicCachePath, music.Id);
                    if (File.Exists(musicCache))
                    { 
                        Common.GlobalArgs.Audio.Play(musicCache);
                        return;
                    }

                    var wifi = Plugin.Connectivity.Abstractions.ConnectionType.WiFi;
                    var connectionTypes = CrossConnectivity.Current.ConnectionTypes;
                    if (!connectionTypes.Contains(wifi))
                    {
                        DependencyService.Get<IToast>().Show("没有WIFI，不能播放");
                        return;
                    }
                    music = _musicNetPlatform.UpdateMusicDetail(music).Result;

                    using (HttpClient client = new HttpClient())
                    {
                        var data = client.GetByteArrayAsync(music.PlayUrl).ConfigureAwait(false).GetAwaiter().GetResult();
                        System.IO.File.WriteAllBytes(musicCache, data);
                    }

                    _myModel.CurrentMusic = music;
                    Common.GlobalArgs.Audio.Play(musicCache);
                }
                catch (Exception ex)
                {
                    DisplayAlert("出错啦", "歌曲信息在本地未找到。。。", "确定");
                }
            });


        }

        private void PlayerStateChange_Clicked(object sender, EventArgs e)
        {
            if (_myModel.IsPlaying == true)
            {
                Common.GlobalArgs.Audio.Pause();
            }
            else
            {
                Common.GlobalArgs.Audio.Start();
            }

            _myModel.IsPlaying = !_myModel.IsPlaying;
        }

        private async void BtnAddToMyFavorite_Clicked(object sender, EventArgs e)
        {
            var musicDetailId = (sender as ImageButton).ClassId;
            var music = await DatabaseProvide.Database.GetAsync<MusicDetail>(musicDetailId);
            _addToMyFavoritePage.Initialize(music);
            await Navigation.PushPopupAsync(_addToMyFavoritePage);
        }
    }
}