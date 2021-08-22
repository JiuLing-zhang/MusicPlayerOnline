using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Player
{
    public delegate void PlaylistChangedEventHandler(object sender);
    public interface IPlayerProvider
    {
        event PlaylistChangedEventHandler PlaylistChanged;

        void AddToPlaylist(PlaylistModel music);
        void RemoveFromPlaylist(int musicId);
        void ClearPlaylist();
        void Play(int musicId);
        void Pause();
        void Previous();
        void Next();
        /// <summary>
        /// 设置播放方式
        /// </summary>
        void SetPlayMode(PlayModeEnum playMode);
    }
}