namespace MusicPlayerOnline.Config
{
    public class Config
    {
        /// <summary>
        /// 常规设置
        /// </summary>
        public GeneralConfig General { get; set; }
        /// <summary>
        /// 播放设置
        /// </summary>
        public PlayConfig Play { get; set; }
        /// <summary>
        /// 播放器设置
        /// </summary>
        public PlayerConfig Player { get; set; }
    }
}
