namespace MusicPlayerOnline.Config
{
    public class PlayConfig
    {
        private bool _isSavePlaylistToLocal;
        /// <summary>
        /// 是否保存播放记录到本地
        /// </summary>
        public bool IsSavePlaylistToLocal
        {
            get => _isSavePlaylistToLocal;
            set
            {
                _isSavePlaylistToLocal = value;
                ConfigHandle.Save();
            }
        }

        private bool _isAutoNextWhenFailed;
        /// <summary>
        /// 歌曲无法播放时自动跳到下一首
        /// </summary>
        public bool IsAutoNextWhenFailed
        {
            get => _isAutoNextWhenFailed;
            set
            {
                _isAutoNextWhenFailed = value;
                ConfigHandle.Save();
            }
        }
    }
}
