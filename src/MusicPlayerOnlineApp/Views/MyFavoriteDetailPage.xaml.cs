using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyFavoriteDetailPage : ContentPage
    {
        private readonly MyFavoriteDetailPageViewModel _myModel = new MyFavoriteDetailPageViewModel();
        private MyFavorite _myFavorite;
        public MyFavoriteDetailPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }

        public void Initialize(MyFavorite myFavorite)
        {
            _myFavorite = myFavorite;
            _myModel.Title = $"歌单: {myFavorite.Name}";
            Task.Run(() =>
            {
                var myFavoriteDetailList = DatabaseProvide.Database.Table<MyFavoriteDetail>().Where(x => x.MyFavoriteId == _myFavorite.Id).ToListAsync().Result;
                int seq = 0;
                foreach (var myFavoriteDetail in myFavoriteDetailList)
                {
                    _myModel.MyFavoriteMusics.Add(new MusicDetailViewModel()
                    {
                        Seq = ++seq,
                        Id = myFavoriteDetail.MusicId,
                        Platform = myFavoriteDetail.Platform.GetDescription(),
                        Artist = myFavoriteDetail.Artist,
                        Album = myFavoriteDetail.Album,
                        Name = myFavoriteDetail.Name
                    });
                }
            });
        }

        private async void MyFavoriteDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}