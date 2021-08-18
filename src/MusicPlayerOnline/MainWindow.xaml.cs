using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Common;
using MusicPlayerOnline.Model.ViewModel;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainWindowViewModel _myModel = new();
        //todo 工厂方式创建
        readonly IMusicProvider _myMusicProvider = new NeteaseMusicProvider();

        public MainWindow()
        {
            InitializeComponent();

            LoadingAppConfig();
            BindingDataForUI();

            _myModel.PlayList.Add(new MusicInfoModel()
            {
                Name = "test1"
            });
            _myModel.PlayList.Add(new MusicInfoModel()
            {
                Name = "test2"
            });
            _myModel.PlayList.Add(new MusicInfoModel()
            {
                Name = "test3"
            });
        }

        private void LoadingAppConfig()
        {

        }

        /// <summary>
        /// UI数据绑定
        /// </summary>
        private void BindingDataForUI()
        {
            ListViewMusicSearchResult.ItemsSource = _myModel.MusicSearchResult;
            ListBoxPlayList.ItemsSource = _myModel.PlayList;
            DataContext = _myModel;
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.ImgMaximize.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/maximize.png"));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.ImgMaximize.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/Themes/Dark/restore.png"));
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            Messages.ShowInfo("设置");
        }

        private void ReadyToSearch_Click(object sender, RoutedEventArgs e)
        {
            TxtKeyword.Focus();
        }

        private void TxtKeyword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _myModel.SearchKeyword = "";
                return;
            }
            if (e.Key != Key.Enter)
            {
                return;
            }
            if (_myModel.SearchKeyword.IsEmpty())
            {
                return;
            }

            Task.Run(() =>
            {
                var result = _myMusicProvider.Search(_myModel.SearchKeyword).Result;
                if (result.Code != 0)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Messages.ShowError(result.Message);
                    });
                    return;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _myModel.MusicSearchResult.Clear();
                    foreach (var musicInfo in result.Data)
                    {
                        _myModel.MusicSearchResult.Add(new MusicInfoModel()
                        {
                            Name = musicInfo.Name,
                            Alias = musicInfo.Alias == "" ? "" : $"（{musicInfo.Alias}）",
                            Artist = musicInfo.ArtistName,
                            Album = musicInfo.AlbumName,
                            Duration = musicInfo.DurationString,
                            Platform = musicInfo.Platform.GetDescription()
                        });
                    }
                });
            });
        }
    }
}
