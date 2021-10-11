using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {

        public SettingPageViewModel()
        {
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "设置";

        private bool _isWifiPlayOnly = true;
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
            }
        }

        private bool _isCloseSearchPageWhenPlayFailed = false;
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
            }
        }

        private bool _isCleanPlaylistWhenPlayMyFavorite = true;
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
            }
        }
    }
}
