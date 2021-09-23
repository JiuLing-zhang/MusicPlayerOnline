using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class AddToMyFavoritePageViewModel : ViewModelBase
    {
        public AddToMyFavoritePageViewModel()
        {
            MyFavoriteList = new ObservableCollection<MyFavoriteViewModel>();
        }

        private ObservableCollection<MyFavoriteViewModel> _myFavoriteList;
        public ObservableCollection<MyFavoriteViewModel> MyFavoriteList
        {
            get => _myFavoriteList;
            set
            {
                _myFavoriteList = value;
                OnPropertyChanged();
            }
        }
    }
}
