using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using MusicPlayerOnlineApp.Views;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    [QueryProperty(nameof(SearchKeyword), nameof(SearchKeyword))]
    public class SearchResultPageViewModel : ViewModelBase
    {
        private readonly ISearchService _searchService;
        private readonly IMusicService _musicService;
        private readonly IPlaylistService _playlistService;

        private string _lastSearchKeyword = "";
        public Command<SearchResultViewModel> AddToMyFavoriteCommand => new Command<SearchResultViewModel>(AddToMyFavorite);
        public Command SelectedChangedCommand => new Command(SearchFinished);
        public SearchResultPageViewModel()
        {
            MusicSearchResult = new ObservableCollection<SearchResultViewModel>();
            _searchService = new SearchService();
            _musicService = new MusicService();
            _playlistService = new PlaylistService();
        }

        private string _title;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }



        public IEnumerable<PlatformEnum> MyEnumTypeValues
        {
            get
            {
                return System.Enum.GetValues(typeof(PlatformEnum)).Cast<PlatformEnum>();
            }
        }

        private string _searchKeyword;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                Search();
            }
        }

        private bool _isMusicSearching;
        /// <summary>
        /// 正在搜索歌曲
        /// </summary>
        public bool IsMusicSearching
        {
            get => _isMusicSearching;
            set
            {
                _isMusicSearching = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SearchResultViewModel> _musicSearchResult;
        /// <summary>
        /// 搜索到的结果列表
        /// </summary>
        public ObservableCollection<SearchResultViewModel> MusicSearchResult
        {
            get => _musicSearchResult;
            set
            {
                _musicSearchResult = value;
                OnPropertyChanged();
            }
        }

        private SearchResultViewModel _musicSelectedResult;
        /// <summary>
        /// 选择的结果集
        /// </summary>
        public SearchResultViewModel MusicSelectedResult
        {
            get => _musicSelectedResult;
            set
            {
                _musicSelectedResult = value;
                OnPropertyChanged();
            }
        }
        private async void Search()
        {
            if (SearchKeyword.IsEmpty())
            {
                return;
            }

            if (SearchKeyword == _lastSearchKeyword)
            {
                return;
            }
            await Logger.WriteAsync(LogTypeEnum.消息, $"准备搜索：{SearchKeyword}");
            _lastSearchKeyword = SearchKeyword;

            try
            {
                IsMusicSearching = true;
                Title = $"搜索: {SearchKeyword}";
                MusicSearchResult.Clear();
                var musics = await _searchService.Search(GlobalArgs.AppConfig.Search.EnablePlatform, SearchKeyword);
                if (musics.Count == 0)
                {
                    await Logger.WriteAsync(LogTypeEnum.消息, $"啥也没有搜到");
                    DependencyService.Get<IToast>().Show("哦吼，啥也没有搜到");
                    return;
                }

                foreach (var musicInfo in musics)
                {
                    if (GlobalArgs.AppConfig.Search.IsHideShortMusic && musicInfo.Duration != 0 && musicInfo.Duration <= 60 * 1000)
                    {
                        await Logger.WriteAsync(LogTypeEnum.消息, $"短歌曲，跳过：{musicInfo.Platform.GetDescription()},{musicInfo.Name}");
                        continue;
                    }

                    MusicSearchResult.Add(new SearchResultViewModel()
                    {
                        Platform = musicInfo.Platform.GetDescription(),
                        Name = musicInfo.Name,
                        Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                        Artist = musicInfo.Artist,
                        Album = musicInfo.Album,
                        Duration = musicInfo.DurationText,
                        SourceData = musicInfo
                    });
                }
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(LogTypeEnum.错误, $"搜索失败：{ex.Message}.{ex.StackTrace}");
                DependencyService.Get<IToast>().Show("抱歉，网络可能出小差了~");
            }
            finally
            {
                IsMusicSearching = false;
            }
        }

        private async void AddToMyFavorite(SearchResultViewModel searchResult)
        {
            GlobalMethods.ShowLoading();
            MusicDetail music;
            try
            {
                bool succeed;
                string message;
                (succeed, message, music) = await SaveMusic(searchResult.SourceData);
                if (succeed == false)
                {
                    if (GlobalArgs.AppConfig.Search.IsCloseSearchPageWhenPlayFailed)
                    {
                        await Shell.Current.GoToAsync("..", true);
                    }
                    DependencyService.Get<IToast>().Show(message);
                    return;
                }
            }
            finally
            {
                GlobalMethods.HideLoading();
            }
            MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
            await Shell.Current.GoToAsync($"{nameof(AddToMyFavoritePage)}?{nameof(AddToMyFavoritePageViewModel.AddedMusicId)}={music.Id}", true);
            await GlobalMethods.PlayMusic(music);
        }

        private async void SearchFinished()
        {
            GlobalMethods.ShowLoading();
            MusicDetail music;
            try
            {
                bool succeed;
                string message;
                await Logger.WriteAsync(LogTypeEnum.消息, $"选择歌曲：{MusicSelectedResult.SourceData.Id}");
                (succeed, message, music) = await SaveMusic(MusicSelectedResult.SourceData);
                if (succeed == false)
                {
                    if (GlobalArgs.AppConfig.Search.IsCloseSearchPageWhenPlayFailed)
                    {
                        await Shell.Current.GoToAsync("..", true);
                    }
                    DependencyService.Get<IToast>().Show(message);
                    return;
                }
            }
            finally
            {
                GlobalMethods.HideLoading();
            }

            if (await GlobalMethods.PlayMusic(music) == false)
            {
                return;
            }
            MessagingCenter.Send(this, SubscribeKey.UpdatePlaylist);
            await Shell.Current.GoToAsync($"..", false);
            await Shell.Current.GoToAsync($"//{nameof(PlayingPage)}", true);
        }

        private async Task<(bool Succeed, string Message, MusicDetail MusicDetailResult)> SaveMusic(MusicSearchResult searchResult)
        {
            var music = await _searchService.GetMusicDetail(searchResult);
            if (music == null)
            {
                await Logger.WriteAsync(LogTypeEnum.消息, "emm没有解析出歌曲信息");
                return (false, "emm没有解析出歌曲信息", null);
            }
            await Logger.WriteAsync(LogTypeEnum.消息, "歌曲解析完成");
            await _musicService.Add(music);
            await _playlistService.Add(music);

            return (true, "", music);
        }
    }
}
