using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MusicPlayerOnline.Win.Common;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Win
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private readonly SettingWindowViewModel _myModel = new();
        public SettingWindow()
        {
            InitializeComponent();
            LoadConfig();
            DataContext = _myModel;
            _myModel.Version = $"版本：{GlobalArgs.AppVersion}";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GoWebsite_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            OpenUrl(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
        private void OpenUrl(string url)
        {
            using (Process compiler = new Process())
            {
                compiler.StartInfo.FileName = url;
                compiler.StartInfo.UseShellExecute = true;
                compiler.Start();
            }
        }
         
        private void BtnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            var app = JiuLing.AutoUpgrade.Shell.AutoUpgradeFactory.Create();
            app.UseHttpMode(Resource.AutoUpgradePath).Run();
        }
        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            var notify = sender as NotifyIcon;
            if (notify == null)
            {
                return;
            }
            OpenUrl(notify.Tag.ToString());
        }
        private void LoadConfig()
        {
            //常规设置
            ChkAutoCheckUpdate.IsChecked = GlobalArgs.AppConfig.General.IsAutoCheckUpdate;
            ChkHideWindowWhenMinimize.IsChecked = GlobalArgs.AppConfig.General.IsHideWindowWhenMinimize;

            //播放设置
            ChkSavePlaylistToLocal.IsChecked = GlobalArgs.AppConfig.Play.IsSavePlaylistToLocal;
            ChkAutoNextWhenFailed.IsChecked = GlobalArgs.AppConfig.Play.IsAutoNextWhenFailed;
        }
        private async void ChkAutoCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            GlobalArgs.AppConfig.General.IsAutoCheckUpdate = Convert.ToBoolean(ChkAutoCheckUpdate.IsChecked);
            await GlobalMethods.WriteGeneralConfig();
        }
        private async void ChkHideWindowWhenMinimize_Click(object sender, RoutedEventArgs e)
        {
            GlobalArgs.AppConfig.General.IsHideWindowWhenMinimize = Convert.ToBoolean(ChkHideWindowWhenMinimize.IsChecked);
            await GlobalMethods.WriteGeneralConfig();
        }

        private async void ChkSavePlaylistToLocal_Click(object sender, RoutedEventArgs e)
        {
            GlobalArgs.AppConfig.Play.IsSavePlaylistToLocal = Convert.ToBoolean(ChkSavePlaylistToLocal.IsChecked);
            await GlobalMethods.WritePlayConfig();
        }

        private async void ChkAutoNextWhenFailed_Click(object sender, RoutedEventArgs e)
        {
            GlobalArgs.AppConfig.Play.IsAutoNextWhenFailed = Convert.ToBoolean(ChkAutoNextWhenFailed.IsChecked);
            await GlobalMethods.WritePlayConfig();
        }
    }
}
