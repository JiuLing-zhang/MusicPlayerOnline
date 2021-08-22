using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Player
{
    public class PlayerProvider : IPlayerProvider
    {
        private readonly MediaPlayer _player = new MediaPlayer();
        private readonly List<PlaylistModel> _playlist = new List<PlaylistModel>();
        /// <summary>
        /// 当前播放的索引
        /// </summary>
        private PlayModeEnum _playMode;

        public event PlaylistChangedEventHandler PlaylistChanged;

        public PlayerProvider()
        {
            _player.MediaOpened += _player_MediaOpened;
            _player.BufferingStarted += _player_BufferingStarted;
            _player.BufferingEnded += _player_BufferingEnded;
            _player.MediaEnded += _player_MediaEnded;
        }

        private void _player_MediaEnded(object sender, EventArgs e)
        {
            Console.WriteLine("播放完成");
        }

        private void _player_BufferingEnded(object sender, EventArgs e)
        {
            Console.WriteLine("缓冲完成");
        }

        private void _player_BufferingStarted(object sender, EventArgs e)
        {
            Console.WriteLine("开始缓冲");
        }

        private void _player_MediaOpened(object sender, EventArgs e)
        {
            Console.WriteLine("媒体打开");
        }

        public void AddToPlaylist(PlaylistModel music)
        {
            if (_playlist.Any(x => x.Id == music.Id))
            {
                return;
            }

            _playlist.Add(music);
            if (PlaylistChanged != null)
            {
                PlaylistChanged(_playlist);
            }
        }

        public void RemoveFromPlaylist(int musicId)
        {
            var music = _playlist.FirstOrDefault(x => x.Id == musicId);
            if (music == null)
            {
                return;
            }

            _playlist.Remove(music);
            if (PlaylistChanged != null)
            {
                PlaylistChanged(_playlist);
            }
        }

        public void ClearPlaylist()
        {
            _playlist.Clear();
            if (PlaylistChanged != null)
            {
                PlaylistChanged(_playlist);
            }
        }

        public void Play(int musicId)
        {
            var music = _playlist.FirstOrDefault(x => x.Id == musicId);
            if (music == null)
            {
                return;
            }

            _player.Open(new Uri(music.PlayUrl));
            _player.Play();

        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 设置播放方式
        /// </summary>
        public void SetPlayMode(PlayModeEnum playMode)
        {
            _playMode = playMode;
        }
    }
}
