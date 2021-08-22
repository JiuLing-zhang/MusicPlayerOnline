using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Player
{
    public delegate void MusicStartedEventHandler(PlaylistModel music);
    public interface IPlayerProvider
    {
        event MusicStartedEventHandler MusicStarted;
        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted { set; }
        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayModeEnum PlayMode { set; }

        void AddToPlaylist(PlaylistModel music);
        void RemoveFromPlaylist(int musicId);
        void ClearPlaylist();
        void PlayNew(int musicId);
        void Play();
        void Pause();
        void Previous();
        void Next();
    }
}