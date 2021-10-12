using MusicPlayerOnlineApp.Common;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        public SettingPageViewModel()
        {
            GetAppConfig();
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "设置";

        private bool _isAutoCheckUpdate;
        /// <summary>
        /// 自动检查更新
        /// </summary>
        public bool IsAutoCheckUpdate
        {
            get => _isAutoCheckUpdate;
            set
            {
                _isAutoCheckUpdate = value;
                OnPropertyChanged();

                GlobalArgs.AppConfig.General.IsAutoCheckUpdate = value;
                WriteGeneralConfig();
            }
        }

        private bool _isWifiPlayOnly;
        /// <summary>
        /// 仅WIFI下可播放
        /// </summary>
        public bool IsWifiPlayOnly
        {
            get => _isWifiPlayOnly;
            set
            {
                _isWifiPlayOnly = value;
                OnPropertyChanged();

                GlobalArgs.AppConfig.Play.IsWifiPlayOnly = value;
                WritePlayConfig();
            }
        }

        private bool _isAutoNextWhenFailed;
        /// <summary>
        /// 播放失败时自动跳到下一首
        /// </summary>
        public bool IsAutoNextWhenFailed
        {
            get => _isAutoNextWhenFailed;
            set
            {
                _isAutoNextWhenFailed = value;
                OnPropertyChanged();

                GlobalArgs.AppConfig.Play.IsAutoNextWhenFailed = value;
                WritePlayConfig();
            }
        }


        private bool _isCloseSearchPageWhenPlayFailed;
        /// <summary>
        /// 播放失败时关闭搜索页面
        /// </summary>
        public bool IsCloseSearchPageWhenPlayFailed
        {
            get => _isCloseSearchPageWhenPlayFailed;
            set
            {
                _isCloseSearchPageWhenPlayFailed = value;
                OnPropertyChanged();

                GlobalArgs.AppConfig.Play.IsCloseSearchPageWhenPlayFailed = value;
                WritePlayConfig();
            }
        }

        private bool _isCleanPlaylistWhenPlayMyFavorite;
        /// <summary>
        /// 播放我的歌单前清空播放列表
        /// </summary>
        public bool IsCleanPlaylistWhenPlayMyFavorite
        {
            get => _isCleanPlaylistWhenPlayMyFavorite;
            set
            {
                _isCleanPlaylistWhenPlayMyFavorite = value;
                OnPropertyChanged();

                GlobalArgs.AppConfig.Play.IsCleanPlaylistWhenPlayMyFavorite = value;
                WritePlayConfig();
            }
        }

        private void GetAppConfig()
        {
            //常规设置
            IsAutoCheckUpdate = GlobalArgs.AppConfig.General.IsAutoCheckUpdate;
            //播放设置
            IsWifiPlayOnly = GlobalArgs.AppConfig.Play.IsWifiPlayOnly;
            IsAutoNextWhenFailed = GlobalArgs.AppConfig.Play.IsAutoNextWhenFailed;
            IsCloseSearchPageWhenPlayFailed = GlobalArgs.AppConfig.Play.IsCloseSearchPageWhenPlayFailed;
            IsCleanPlaylistWhenPlayMyFavorite = GlobalArgs.AppConfig.Play.IsCleanPlaylistWhenPlayMyFavorite;
        }

        /// <summary>
        /// 保存通用配置
        /// </summary>
        private async void WriteGeneralConfig()
        {
            await GlobalMethods.WriteGeneralConfig();
        }

        /// <summary>
        /// 保存播放配置
        /// </summary>
        private async void WritePlayConfig()
        {
            await GlobalMethods.WritePlayConfig();
        }
    }
}
