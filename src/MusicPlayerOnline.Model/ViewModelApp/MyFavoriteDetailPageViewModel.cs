using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
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
