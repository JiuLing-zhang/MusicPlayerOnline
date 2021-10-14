using System;
using Android.Media;
using Java.Lang;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]
namespace MusicPlayerOnlineApp.Droid
{
    public class AudioService : IAudio
    {
        private MediaPlayer _player;
        public event MediaBeginEventHandler MediaBegin;
        public event MediaEndedEventHandler MediaEnded;
        public event MediaFailedEventHandler MediaFailed;
        public AudioService()
        {
            InitMediaPlayer();
        }

        private void InitMediaPlayer()
        {
            _player = new MediaPlayer();
            _player.Prepared += (s, e) =>
            {
                try
                {
                    _player.Start();
                }
                catch (IllegalStateException ex)
                {
                }
            };
            _player.Completion += (sender, e) =>
            {
                _player.Release();
                MediaEnded?.Invoke();
            };
            _player.Error += (sender, e) =>
            {
                MediaFailed?.Invoke();
            };
        }

        public bool IsPlaying => CheckIsPlaying();

        private bool CheckIsPlaying()
        {
            try
            {
                return _player.IsPlaying;
            }
            catch (IllegalStateException ex)
            {
                //未初始化时，说明已经播放结束，当前未播放
                return false;
            }
        }

        public async void Play(string path)
        {
            if (CheckIsPlaying())
            {
                Stop();
            }

            try
            {
                await _player.SetDataSourceAsync(path);
                MediaBegin?.Invoke();
                _player.PrepareAsync();
            }
            catch (IllegalStateException ex)
            {
                //这里处理一下播放器状态不一致导致的无法播放的问题
                InitMediaPlayer();
                Play(path);
            }
        }

        public void Pause()
        {
            try
            {
                _player.Pause();
            }
            catch (IllegalStateException ex)
            {
            }

        }
        /// <summary>
        /// 恢复播放
        /// </summary>
        public void Start()
        {
            try
            {
                _player.Start();
            }
            catch (IllegalStateException ex)
            {
            }
        }

        public void Stop()
        {
            try
            {
                _player.Stop();
                _player.Reset();
            }
            catch (IllegalStateException ex)
            {
                //未初始化时不用停止
            }
        }
        public void SeekTo(int millisecond)
        {
            _player.SeekTo(millisecond);
        }

        public void Dispose()
        {
            _player.Dispose();
        }

        public (int Duration, int Position) GetPosition()
        {
            return (_player.Duration, _player.CurrentPosition);
        }
    }
}