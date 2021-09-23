using System.Collections.ObjectModel;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        public string Title => "我的歌单";
        public MyFavoritePageViewModel()
        {
            FavoriteList = new ObservableCollection<MyFavoriteViewModel>();
        }

        private ObservableCollection<MyFavoriteViewModel> _favoriteList;
        public ObservableCollection<MyFavoriteViewModel> FavoriteList
        {
            get => _favoriteList;
            set
            {
                _favoriteList = value;
                OnPropertyChanged();
            }
        }
    }
}
