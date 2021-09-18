using System;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class PlaylistViewModel : ViewModelBase
    {
        private string _id;
        /// <summary>
        /// id
        /// </summary>
        public string Id
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

        private string _imageUrl;
        /// <summary>
        /// 歌曲图片地址
        /// </summary>
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }

        private MusicDetail _sourceData;
        /// <summary>
        /// 源数据，对应的MusicSearchResult
        /// </summary>
        public MusicDetail SourceData
        {
            get => _sourceData;
            set
            {
                _sourceData = value;
                OnPropertyChanged();
            }
        }
    }
}
