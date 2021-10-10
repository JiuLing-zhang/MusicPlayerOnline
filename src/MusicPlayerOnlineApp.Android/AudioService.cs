using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]
namespace MusicPlayerOnlineApp.Droid
{
    public class AudioService : IAudio
    {
        private readonly MediaPlayer _player;
        public event MediaBeginEventHandler MediaBegin;
        public event MediaEndedEventHandler MediaEnded;
        public event MediaFailedEventHandler MediaFailed;
        public AudioService()
        {
            _player = new MediaPlayer();

            _player.Prepared += (s, e) =>
            {
                _player.Start();
            };
            _player.Completion += (sender, e) =>
            {
                MediaEnded?.Invoke();
                _player.Release();
            };
            _player.Error += (sender, e) =>
            {
                MediaFailed?.Invoke();
            };
        }
        public async void Play(string path)
        {
            Stop();
            await _player.SetDataSourceAsync(path);
            MediaBegin?.Invoke();
            _player.PrepareAsync();
        }

        public void Pause()
        {
            _player.Pause();
        }
        /// <summary>
        /// 恢复播放
        /// </summary>
        public void Start()
        {
            _player.Start();
        }

        public void Stop()
        {
            if (_player.IsPlaying)
            {
                _player.Stop();
                _player.Reset();
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
    }
}