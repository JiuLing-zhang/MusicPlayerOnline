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
using MusicPlayerOnline.Player;
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
                _player.Release();
            };
        }


        public async void Play(string path)
        {
            await _player.SetDataSourceAsync(path);
            _player.PrepareAsync();
        }

        public void Pause()
        {
            _player.Pause();
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