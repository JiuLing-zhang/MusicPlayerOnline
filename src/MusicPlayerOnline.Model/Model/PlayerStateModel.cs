using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Model.Model
{
    public class PlayerStateModel
    {
        public PlayModeEnum PlayMode { get; set; }
        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMute { get; set; }
        /// <summary>
        /// 正在播放
        /// </summary>
        public bool IsPlaying { get; set; }
    }
}
