﻿using System;

namespace MusicPlayerOnline.App.AppInterface
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

        bool IsPlaying { get; }
        void Play(string path);
        void Pause();
        /// <summary>
        /// 恢复播放
        /// </summary>
        void Start();
        void Stop();
        void SeekTo(int millisecond);

        void Dispose();

        (int Duration, int Position) GetPosition();
    }
}
