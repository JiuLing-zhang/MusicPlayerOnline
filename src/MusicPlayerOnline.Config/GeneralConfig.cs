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
    }
}
