namespace MusicPlayerOnlineApp.ViewModels
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
