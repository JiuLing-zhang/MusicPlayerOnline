namespace MusicPlayerOnline.Model.ViewModel
{
    public class MusicInfoViewModel : ViewModelBase
    {
        private int _id;
        /// <summary>
        /// 歌曲id
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _alias;
        /// <summary>
        /// 歌曲别名
        /// </summary>
        public string Alias
        {
            get => _alias;
            set
            {
                _alias = value;
                OnPropertyChanged();
            }
        }


        private string _artist;
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }

        private string _album;
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string Album
        {
            get => _album;
            set
            {
                _album = value;
                OnPropertyChanged();
            }
        }

        private string _duration;
        /// <summary>
        /// 时长
        /// </summary>
        public string Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        private string _platform;

        /// <summary>
        /// 平台
        /// </summary>
        public string Platform
        {
            get => _platform;
            set
            {
                _platform = value;
                OnPropertyChanged();
            }
        }


        private string _playUrl;

        /// <summary>
        /// 播放地址
        /// </summary>
        public string PlayUrl
        {
            get => _playUrl;
            set
            {
                _playUrl = value;
                OnPropertyChanged();
            }
        }
    }
}
