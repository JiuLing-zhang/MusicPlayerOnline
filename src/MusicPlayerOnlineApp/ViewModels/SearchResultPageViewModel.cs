using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Service;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class SearchResultPageViewModel : ViewModelBase
    {
        private ISearchService _searchService;
        public SearchResultPageViewModel()
        {
            MusicSearchResult = new ObservableCollection<SearchResultViewModel>();
            _searchService = new SearchService();
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

        private PlatformEnum _searchPlatform = PlatformEnum.Netease;
        /// <summary>
        /// 搜索平台
        /// </summary>
        public PlatformEnum SearchPlatform
        {
            get => _searchPlatform;
            set
            {
                _searchPlatform = value;
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
                SearchFinished();
            }
        }

        private async void Search()
        {
            try
            {
                IsMusicSearching = true;
                Title = $"搜索: {SearchKeyword}";
                MusicSearchResult.Clear();
                var musics = await _searchService.Search(SearchPlatform, SearchKeyword);
                if (musics.Count == 0)
                {
                    return;
                }

                foreach (var musicInfo in musics)
                {
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
            catch (Exception e)
            {
                //TODO 处理异常
                throw e;
            }
            finally
            {
                IsMusicSearching = false;
            }
        }
        private async void SearchFinished()
        {
            await _searchService.SaveResultToPlaylist(MusicSelectedResult.SourceData);
            //TODO play
        }
    }
}
