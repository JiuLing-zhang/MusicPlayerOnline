using System;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Player
{
    public delegate void MusicMediaFailedEventHandler();
    public delegate void MediaEndedEventHandler();
    public interface IPlayerProvider
    {
        /// <summary>
        /// 播放失败
        /// </summary>
        public event MusicMediaFailedEventHandler MusicMediaFailed;

        public event MediaEndedEventHandler MediaEnded;
        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted { set; }

        /// <summary>
        /// 声音大小
        /// </summary>
        public double VoiceValue { set; }
         
        void Play(MusicDetail music = null);
        void Pause();
        void SetProgress(double percent);
        (bool isPlaying, TimeSpan position, TimeSpan total, double percent) GetPosition();
    }
}