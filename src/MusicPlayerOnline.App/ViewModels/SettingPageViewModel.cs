using System.Windows.Input;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Common;
using MusicPlayerOnline.App.Views;
using MusicPlayerOnline.Model.Enum;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        public ICommand OpenUrlCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public ICommand ClearCacheCommand => new Command(ClearCache);
        public ICommand OpenLogCommand => new Command(OpenLog);
        public SettingPageViewModel()
        {
            GetAppConfig();
            VersionName = DependencyService.Get<IAppVersionInfo>().GetVersionName();
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

        private bool _isEnableNetease;
        /// <summary>
        /// 网易云
        /// </summary>
        public bool IsEnableNetease
        {
            get => _isEnableNetease;
            set
            {
                _isEnableNetease = value;
                OnPropertyChanged();

                EnableNetease();
            }
        }

        private bool _isEnableKuGou;
        /// <summary>
        /// 酷狗
        /// </summary>
        public bool IsEnableKuGou
        {
            get => _isEnableKuGou;
            set
            {
                _isEnableKuGou = value;
                OnPropertyChanged();

                EnableKuGou();
            }
        }

        private bool _isEnableMiGu;
        /// <summary>
        /// 咪咕
        /// </summary>
        public bool IsEnableMiGu
        {
            get => _isEnableMiGu;
            set
            {
                _isEnableMiGu = value;
                OnPropertyChanged();

                EnableMiGu();
            }
        }

        private bool _isHideShortMusic;
        /// <summary>
        /// 隐藏小于1分钟的歌曲
        /// </summary>
        public bool IsHideShortMusic
        {
            get => _isHideShortMusic;
            set
            {
                _isHideShortMusic = value;
                OnPropertyChanged();

                GlobalArgs.AppConfig.Search.IsHideShortMusic = value;
                WritePlatformConfig();
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

                GlobalArgs.AppConfig.Search.IsCloseSearchPageWhenPlayFailed = value;
                WritePlatformConfig();
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

        private string _versionName;
        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionName
        {
            get => _versionName;
            set
            {
                _versionName = value;
                OnPropertyChanged();
            }
        }

        private void GetAppConfig()
        {
            //常规设置
            IsAutoCheckUpdate = GlobalArgs.AppConfig.General.IsAutoCheckUpdate;

            //搜索平台设置
            IsEnableNetease = CheckEnablePlatform(PlatformEnum.Netease);
            IsEnableKuGou = CheckEnablePlatform(PlatformEnum.KuGou);
            IsEnableMiGu = CheckEnablePlatform(PlatformEnum.MiGu);
            IsHideShortMusic = GlobalArgs.AppConfig.Search.IsHideShortMusic;
            IsCloseSearchPageWhenPlayFailed = GlobalArgs.AppConfig.Search.IsCloseSearchPageWhenPlayFailed;

            //播放设置
            IsWifiPlayOnly = GlobalArgs.AppConfig.Play.IsWifiPlayOnly;
            IsAutoNextWhenFailed = GlobalArgs.AppConfig.Play.IsAutoNextWhenFailed;
            IsCleanPlaylistWhenPlayMyFavorite = GlobalArgs.AppConfig.Play.IsCleanPlaylistWhenPlayMyFavorite;
        }

        private bool CheckEnablePlatform(PlatformEnum platform)
        {
            if ((GlobalArgs.AppConfig.Search.EnablePlatform & platform) == platform)
            {
                return true;
            }

            return false;
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

        private async void EnableNetease()
        {
            if (IsEnableNetease)
            {
                if (!CheckEnablePlatform(PlatformEnum.Netease))
                {
                    GlobalArgs.AppConfig.Search.EnablePlatform = GlobalArgs.AppConfig.Search.EnablePlatform | PlatformEnum.Netease;
                }
            }
            else
            {
                if (CheckEnablePlatform(PlatformEnum.Netease))
                {
                    GlobalArgs.AppConfig.Search.EnablePlatform = GlobalArgs.AppConfig.Search.EnablePlatform & ~PlatformEnum.Netease;
                }
            }
            WritePlatformConfig();
        }

        private async void EnableKuGou()
        {
            if (IsEnableKuGou)
            {
                if (!CheckEnablePlatform(PlatformEnum.KuGou))
                {
                    GlobalArgs.AppConfig.Search.EnablePlatform = GlobalArgs.AppConfig.Search.EnablePlatform | PlatformEnum.KuGou;
                }
            }
            else
            {
                if (CheckEnablePlatform(PlatformEnum.KuGou))
                {
                    GlobalArgs.AppConfig.Search.EnablePlatform = GlobalArgs.AppConfig.Search.EnablePlatform & ~PlatformEnum.KuGou;
                }
            }
            WritePlatformConfig();
        }

        private async void EnableMiGu()
        {
            if (IsEnableMiGu)
            {
                if (!CheckEnablePlatform(PlatformEnum.MiGu))
                {
                    GlobalArgs.AppConfig.Search.EnablePlatform = GlobalArgs.AppConfig.Search.EnablePlatform | PlatformEnum.MiGu;
                }
            }
            else
            {
                if (CheckEnablePlatform(PlatformEnum.MiGu))
                {
                    GlobalArgs.AppConfig.Search.EnablePlatform = GlobalArgs.AppConfig.Search.EnablePlatform & ~PlatformEnum.MiGu;
                }
            }
            WritePlatformConfig();
        }

        private async void WritePlatformConfig()
        {
            await GlobalMethods.WritePlatformConfig();
        }

        private async void ClearCache()
        {
            await Shell.Current.GoToAsync($"{nameof(ClearCachePage)}", true);
        }

        private async void OpenLog()
        {
            await Shell.Current.GoToAsync($"{nameof(LogPage)}", true);
        }
    }
}
