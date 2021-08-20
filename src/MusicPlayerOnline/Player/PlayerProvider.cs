using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Player
{
    public class PlayerProvider : IPlayerProvider
    {
        private readonly MediaPlayer _player = new MediaPlayer();
        private readonly List<PlaylistModel> _playlist = new List<PlaylistModel>();

        public event PlaylistChangedEventHandler PlaylistChanged;

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
            throw new NotImplementedException();
        }

        public void ClearPlaylist()
        {
            throw new NotImplementedException();
        }

        public void Play(int musicId)
        {
            var music = _playlist.FirstOrDefault(x => x.Id == musicId);
            if (music==null)
            {
                return;
            }

            _player.Open(new Uri(music.PlayUrl));
            _player.Play();

        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

    }
}
