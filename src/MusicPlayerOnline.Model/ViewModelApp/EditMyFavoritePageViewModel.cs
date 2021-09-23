using System;
using System.Collections.Generic;
using System.Text;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class EditMyFavoritePageViewModel : ViewModelBase
    {
        private string _myFavoriteId;
        public string MyFavoriteId
        {
            get => _myFavoriteId;
            set
            {
                _myFavoriteId = value;
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

        private string _newName;
        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                OnPropertyChanged();
            }
        }
    }
}
