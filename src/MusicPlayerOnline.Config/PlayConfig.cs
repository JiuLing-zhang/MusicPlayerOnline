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
    }
}
