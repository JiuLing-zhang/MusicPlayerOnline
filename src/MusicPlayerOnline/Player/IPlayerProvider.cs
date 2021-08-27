using System;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Player
{
    public delegate void MusicStartedEventHandler(MusicDetail music);
    public delegate void MusicMediaFailedEventHandler();
    public interface IPlayerProvider
    {
        event MusicStartedEventHandler MusicStarted;
        /// <summary>
        /// 播放失败
        /// </summary>
        event MusicMediaFailedEventHandler MusicMediaFailed;
        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted { set; }
        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayModeEnum PlayMode { set; }

        /// <summary>
        /// 声音大小
        /// </summary>
        public double VoiceValue { set; }

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying { get; set; }

        void AddToPlaylist(MusicDetail music);
        void RemoveFromPlaylist(string musicId);
        void ClearPlaylist();
        void PlayNew(MusicDetail music);
        void Play();
        void Pause();
        void SetProgress(double percent);
        void Previous();
        void Next();
        (bool isPlaying, TimeSpan position, TimeSpan total, double percent) GetPosition();
    }
}