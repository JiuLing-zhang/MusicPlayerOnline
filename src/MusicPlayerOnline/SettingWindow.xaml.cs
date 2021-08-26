using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MusicPlayerOnline.Common;
using MusicPlayerOnline.Config;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline
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

        private bool _isCheckUpdate = false;
        private void BtnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_isCheckUpdate)
            {
                return;
            }
            _isCheckUpdate = true;
            _myModel.UpdateMessage = "正在查找更新....";
            Task.Run(() =>
            {
                try
                {
                    var (isNewVersion, version, link) = new CheckForUpdates().Check();
                    if (isNewVersion == false)
                    {
                        _myModel.UpdateMessage = "当前版本已经是最新版本！";
                        return;
                    }

                    _myModel.UpdateMessage = $"发现新版本：{version}";
                    var notify = new NotifyIcon
                    {
                        BalloonTipText = $"发现新版本：{version}{System.Environment.NewLine}点击更新",
                        Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath),
                        Tag = link,
                        Visible = true
                    };
                    notify.BalloonTipClicked += notifyIcon_BalloonTipClicked;
                    notify.ShowBalloonTip(5000);
                }
                catch (Exception ex)
                {
                    _myModel.UpdateMessage = $"检查更新失败，{ex.Message}";
                }
                finally
                {
                    _isCheckUpdate = false;
                }
            });
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
            ChkAutoCheckUpdate.IsChecked = AppSetting.Setting.General.IsAutoCheckUpdate;
            ChkHideWindowWhenMinimize.IsChecked = AppSetting.Setting.General.IsHideWindowWhenMinimize;

            //播放设置
            ChkSavePlaylistToLocal.IsChecked = AppSetting.Setting.Play.IsSavePlaylistToLocal;
        }
        private void ChkAutoCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            AppSetting.Setting.General.IsAutoCheckUpdate = Convert.ToBoolean(ChkAutoCheckUpdate.IsChecked);
        }
        private void ChkHideWindowWhenMinimize_Click(object sender, RoutedEventArgs e)
        {
            AppSetting.Setting.General.IsHideWindowWhenMinimize = Convert.ToBoolean(ChkHideWindowWhenMinimize.IsChecked);
        }

        private void ChkSavePlaylistToLocal_Click(object sender, RoutedEventArgs e)
        {
            AppSetting.Setting.Play.IsSavePlaylistToLocal = Convert.ToBoolean(ChkSavePlaylistToLocal.IsChecked);
        }
    }
}
