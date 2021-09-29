using System.Collections.ObjectModel;

namespace MusicPlayerOnlineApp.ViewModels
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
