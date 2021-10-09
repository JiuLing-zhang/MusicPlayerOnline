namespace MusicPlayerOnline.Config
{
    public class GeneralConfig
    {
        private bool _isAutoCheckUpdate;
        /// <summary>
        /// 是否自动检查更新
        /// </summary>
        public bool IsAutoCheckUpdate
        {
            get => _isAutoCheckUpdate;
            set
            {
                _isAutoCheckUpdate = value;
                ConfigHandle.Save();
            }
        }

        private bool _isHideWindowWhenMinimize;
        /// <summary>
        /// 关闭时最小化到托盘
        /// </summary>
        public bool IsHideWindowWhenMinimize
        {
            get => _isHideWindowWhenMinimize;
            set
            {
                _isHideWindowWhenMinimize = value;
                ConfigHandle.Save();
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
                //ConfigHandle.Save();
            }
        }
    }
}
