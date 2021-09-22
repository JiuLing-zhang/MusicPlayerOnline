using System;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class PlaylistViewModel : ViewModelBase
    {
        private string _musicDetailId;
        /// <summary>
        /// id
        /// </summary>
        public string MusicDetailId
        {
            get => _musicDetailId;
            set
            {
                _musicDetailId = value;
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
    }
}
