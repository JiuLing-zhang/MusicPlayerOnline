using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Win.Player
{
    public class PlayerProvider : IPlayerProvider
    {
        public event MusicMediaFailedEventHandler MusicMediaFailed;
        public event MediaEndedEventHandler MediaEnded;

        private readonly MediaPlayer _player;

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted
        {
            set => _player.IsMuted = value;
        }


        public double VoiceValue
        {
            set => _player.Volume = value;
        }

        public PlayerProvider()
        {
            _player = new MediaPlayer();
            _player.MediaEnded += _player_MediaEnded;
            _player.MediaFailed += _player_MediaFailed;
        }

        private void _player_MediaFailed(object sender, ExceptionEventArgs e)
        {
            MusicMediaFailed?.Invoke();
        }

        private void _player_MediaEnded(object sender, EventArgs e)
        {
            MediaEnded?.Invoke();
        }

        public void Play(MusicDetail music = null)
        {
            if (music != null)
            {
                _player.Open(new Uri(music.PlayUrl));
            }
            _player.Play();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void SetProgress(double percent)
        {
            var nd = _player.NaturalDuration;
            if (nd.HasTimeSpan == false)
            {
                return;
            }
            double totalTime = nd.TimeSpan.TotalSeconds;
            var ts = TimeSpan.FromSeconds(totalTime * percent);
            _player.Position = ts;
        }

        public (bool isPlaying, TimeSpan position, TimeSpan total, double percent) GetPosition()
        {
            var nd = _player.NaturalDuration;
            if (nd.HasTimeSpan == false)
            {
                return (false, default, default, default);
            }

            var p = _player.Position;
            double playedTime = p.TotalSeconds;
            double totalTime = nd.TimeSpan.TotalSeconds;
            return (true, p, nd.TimeSpan, playedTime / totalTime);
        }
    }
}
