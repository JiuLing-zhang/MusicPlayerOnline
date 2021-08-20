using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.ViewModel
{
    public class PlaylistViewModel : ViewModelBase
    {

        private int _id;
        /// <summary>
        /// id
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

        private string _musicText;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string MusicText
        {
            get => _musicText;
            set
            {
                _musicText = value;
                OnPropertyChanged();
            }
        }

        private string _musicToolTip;
        /// <summary>
        /// 歌曲提示
        /// </summary>
        public string MusicToolTip
        {
            get => _musicToolTip;
            set
            {
                _musicToolTip = value;
                OnPropertyChanged();
            }
        }


        private bool _isPlaying;
        /// <summary>
        /// 播放中
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
