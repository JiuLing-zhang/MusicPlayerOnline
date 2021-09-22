using System.Collections.ObjectModel;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class PlaylistPageViewModel : ViewModelBase
    {
        public PlaylistPageViewModel()
        {
            Playlist = new ObservableCollection<PlaylistViewModel>();
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "播放列表";

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

        private ObservableCollection<PlaylistViewModel> _playlist;
        /// <summary>
        /// 搜索到的结果列表
        /// </summary>
        public ObservableCollection<PlaylistViewModel> Playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        private MusicDetail _currentMusic;
        /// <summary>
        /// 当前播放的歌曲
        /// </summary>
        public MusicDetail CurrentMusic
        {
            get => _currentMusic;
            set
            {
                _currentMusic = value;
                OnPropertyChanged();
            }
        }


        private bool _isPlaying;
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }
    }
}
