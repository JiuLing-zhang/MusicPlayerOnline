using System.Collections.ObjectModel;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class MyFavoriteDetailPageViewModel : ViewModelBase
    {
        public MyFavoriteDetailPageViewModel()
        {
            MyFavoriteMusics = new ObservableCollection<MusicDetailViewModel>();
        }

        private string _title;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MusicDetailViewModel> _myFavoriteMusics;
        public ObservableCollection<MusicDetailViewModel> MyFavoriteMusics
        {
            get => _myFavoriteMusics;
            set
            {
                _myFavoriteMusics = value;
                OnPropertyChanged();
            }
        }
    }
}
