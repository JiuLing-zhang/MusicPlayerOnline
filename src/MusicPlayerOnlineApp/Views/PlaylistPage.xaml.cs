using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnline.Network;
using MusicPlayerOnlineApp.Common;
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

            _searchResultPage.SelectedFinished = async selectMusic =>
            {
                await SelectedFinishedDo(selectMusic);
            };

            this.Appearing += (sender, args) =>
            {
                RefreshPage();
            };
        }
        private async void RefreshPage()
        {
            var playlist = await DatabaseProvide.Database.Table<Playlist>().ToListAsync();
            _myModel.Playlist.Clear();
            foreach (var m in playlist)
            {
                _myModel.Playlist.Add(new MusicDetailViewModel()
                {
                    Id = m.MusicDetailId,
                    Name = m.Name,
                    Artist = m.Artist
                });
            }
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

        private async Task SelectedFinishedDo(MusicDetail music)
        {
            _myModel.SearchKeyword = "";
            if (await DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == music.Id).CountAsync() == 0)
            {
                await DatabaseProvide.Database.InsertAsync(music);
            }

            if (await DatabaseProvide.Database.Table<Playlist>().Where(x => x.MusicDetailId == music.Id).CountAsync() == 0)
            {
                await DatabaseProvide.Database.InsertAsync(new Playlist() { MusicDetailId = music.Id, Name = music.Name, Artist = music.Artist });
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
            await PlayMusic(music.Id);
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MusicDetailViewModel selectedMusic = e.CurrentSelection[0] as MusicDetailViewModel;
            await PlayMusic(selectedMusic.Id);
        }

        /// <summary>
        /// 播放
        /// </summary>
        private async Task PlayMusic(string id)
        {
            try
            {
                var music = await DatabaseProvide.Database.Table<MusicDetail>().Where(x => x.Id == id).FirstOrDefaultAsync();
                if (music == null)
                {
                    DependencyService.Get<IToast>().Show("未找到歌曲信息");
                    return;
                }

                string musicCache = Path.Combine(Common.GlobalArgs.AppMusicCachePath, music.Id);
                if (File.Exists(musicCache))
                {
                    GlobalArgs.CurrentMusic = music;
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
                music = await _musicNetPlatform.UpdateMusicDetail(music);

                using (HttpClient client = new HttpClient())
                {
                    var data = await client.GetByteArrayAsync(music.PlayUrl);
                    System.IO.File.WriteAllBytes(musicCache, data);
                }

                GlobalArgs.CurrentMusic = music;
                Common.GlobalArgs.Audio.Play(musicCache);
            }
            catch (Exception ex)
            {
                await DisplayAlert("出错啦", "歌曲信息在本地未找到。。。", "确定");
            }
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