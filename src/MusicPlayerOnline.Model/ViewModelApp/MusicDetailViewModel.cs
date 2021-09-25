using System;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    //TODO 这里需要修改文件名
    public class MusicDetailViewModel : ViewModelBase
    {
        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
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

        private int _seq;
        /// <summary>
        /// 序号
        /// </summary>
        public int Seq
        {
            get => _seq;
            set
            {
                _seq = value;
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
    }
}
