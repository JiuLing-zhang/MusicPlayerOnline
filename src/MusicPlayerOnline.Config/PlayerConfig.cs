using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Config
{
    public class PlayerConfig
    {
        private double _voice;
        /// <summary>
        /// 音量
        /// </summary>
        public double Voice
        {
            get => _voice;
            set
            {
                _voice = value;
                ConfigHandle.Save();
            }
        }

        private bool _isSoundOff;
        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsSoundOff
        {
            get => _isSoundOff;
            set
            {
                _isSoundOff = value;
                ConfigHandle.Save();
            }
        }

        private PlayModeEnum _playMode;
        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayModeEnum PlayMode
        {
            get => _playMode;
            set
            {
                _playMode = value;
                ConfigHandle.Save();
            }
        }
    }
}
