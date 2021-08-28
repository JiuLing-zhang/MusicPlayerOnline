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

        private string _currentMusicInfo;
        /// <summary>
        /// 当前播放的信息
        /// </summary>
        public string CurrentMusicInfo
        {
            get => _currentMusicInfo;
            set
            {
                _currentMusicInfo = value;
                OnPropertyChanged();
            }
        }

        private double _voiceValue;
        /// <summary>
        /// 声音大小
        /// </summary>
        public double VoiceValue
        {
            get => _voiceValue;
            set
            {
                _voiceValue = value;
                OnPropertyChanged();
            }
        }


        private string _playedTime;
        /// <summary>
        /// 已播放时长
        /// </summary>
        public string PlayedTime
        {
            get => _playedTime;
            set
            {
                _playedTime = value;
                OnPropertyChanged();
            }
        }

        private string _totalTime;
        /// <summary>
        /// 总时长
        /// </summary>
        public string TotalTime
        {
            get => _totalTime;
            set
            {
                _totalTime = value;
                OnPropertyChanged();
            }
        }


        private double _playPercent;
        /// <summary>
        /// 总时长
        /// </summary>
        public double PlayPercent
        {
            get => _playPercent;
            set
            {
                _playPercent = value;
                OnPropertyChanged();
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
    }
}
