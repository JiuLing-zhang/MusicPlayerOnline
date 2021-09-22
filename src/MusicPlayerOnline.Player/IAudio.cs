namespace MusicPlayerOnline.Player
{
    public delegate void MediaBeginEventHandler();
    public delegate void MediaEndedEventHandler();
    public delegate void MediaFailedEventHandler();
    public interface IAudio
    {
        event MediaBeginEventHandler MediaBegin;
        event MediaEndedEventHandler MediaEnded;
        /// <summary>
        /// 播放失败
        /// </summary>
        event MediaFailedEventHandler MediaFailed;
        void Play(string path);
        void Pause();
        void Stop();
        void SeekTo(int millisecond);

        void Dispose();
    }
}
