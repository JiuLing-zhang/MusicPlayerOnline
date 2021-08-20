using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Model.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            MusicSearchResult = new ObservableCollection<SearchResultViewModel>();
            Playlist = new ObservableCollection<PlaylistViewModel>();
        }

        private PlatformEnum _searchPlatform;
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

        private ObservableCollection<PlaylistViewModel> _playList;
        /// <summary>
        /// 播放列表
        /// </summary>
        public ObservableCollection<PlaylistViewModel> Playlist
        {
            get => _playList;
            set
            {
                _playList = value;
                OnPropertyChanged();
            }
        }
    }
}
