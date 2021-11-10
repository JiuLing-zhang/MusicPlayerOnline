﻿using JiuLing.CommonLibs.ExtensionMethods;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class MyFavoriteViewModel : ViewModelBase
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

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }
         
        public bool IsUseDefaultImage => ImageUrl.IsEmpty();

        private int _musicCount;
        public int MusicCount
        {
            get => _musicCount;
            set
            {
                _musicCount = value;
                OnPropertyChanged();
            }
        }
    }
}
