using System;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnline.Network;
using MusicPlayerOnlineApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultPage : ContentPage
    {
        private readonly SearchResultPageViewModel _myModel = new SearchResultPageViewModel();
        private readonly MusicNetPlatform _musicNetPlatform = new MusicNetPlatform();

        public MusicDetail SelectedMusicDetail;
        public SearchResultPage()
        {
            InitializeComponent();
            BindingContext = _myModel;
        }

        public void Search(string keyword)
        {
            Task.Run(() =>
            {
                try
                {
                    _myModel.Title = $"搜索: {keyword}";
                    _myModel.IsMusicSearching = true;
                    _myModel.SearchKeyword = keyword;
                    _myModel.MusicSearchResult.Clear();

                    SelectedMusicDetail = null;

                    _myModel.SearchPlatform = 0;
                    foreach (PlatformEnum item in Enum.GetValues(typeof(PlatformEnum)))
                    {
                        _myModel.SearchPlatform = _myModel.SearchPlatform | item;
                    }

                    //TODO 开发阶段暂时只用一个平台
                    _myModel.SearchPlatform = PlatformEnum.Netease;

                    var musics = _musicNetPlatform.Search(_myModel.SearchPlatform, keyword).Result;
                    if (musics.Count == 0)
                    {
                        return;
                    }

                    foreach (var musicInfo in musics)
                    {
                        _myModel.MusicSearchResult.Add(new SearchResultViewModel()
                        {
                            Platform = musicInfo.Platform.GetDescription(),
                            Name = musicInfo.Name,
                            Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                            Artist = musicInfo.Artist,
                            Album = musicInfo.Album,
                            Duration = musicInfo.DurationText,
                            SourceData = musicInfo
                        });
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("出错啦", "我也很无奈，不然，重新试试？", "确定");
                }
                finally
                {
                    _myModel.IsMusicSearching = false;
                }
            });
        }

        private void SearchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchResultViewModel selectedMusic = e.CurrentSelection[0] as SearchResultViewModel;
            BuildMusicDetail(selectedMusic.SourceData, musicDetail =>
            {
                if (musicDetail == null)
                {
                    DependencyService.Get<IToast>().Show("该歌曲的信息似乎没找到~~~");
                    return;
                }

                SelectedMusicDetail = musicDetail;
                Navigation.PopAsync();
            });
        }
        private void BuildMusicDetail(MusicSearchResult music, Action<MusicDetail> callback)
        {
            Task.Run(() =>
            {
                var data = _musicNetPlatform.BuildMusicDetail(music).Result;
                callback(data);
            });
        }
    }
}