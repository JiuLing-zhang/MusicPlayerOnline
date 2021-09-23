using System;
using System.Collections.Generic;
using System.Text;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class AddMyFavoritePageViewModel : ViewModelBase
    {
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
    }
}
