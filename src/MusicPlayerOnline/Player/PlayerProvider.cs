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
        public event MusicStartedEventHandler MusicStarted;

        private readonly MediaPlayer _player;
        private readonly List<MusicDetail2> _playlist;
        /// <summary>
        /// 当前播放的id
        /// </summary>
        private string _currentMusicId;

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted
        {
            set => _player.IsMuted = value;
        }

        public PlayModeEnum PlayMode { get; set; }

        public double VoiceValue
        {
            set => _player.Volume = value;
        }
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying { get; set; }

        public PlayerProvider()
        {
            _player = new MediaPlayer();
            _playlist = new List<MusicDetail2>();
        }
        public void AddToPlaylist(MusicDetail2 music)
        {
            if (_playlist.Any(x => x.Id == music.Id))
            {
                return;
            }

            _playlist.Add(music);
        }

        public void RemoveFromPlaylist(string musicId)
        {
            var music = _playlist.FirstOrDefault(x => x.Id == musicId);
            if (music == null)
            {
                return;
            }

            _playlist.Remove(music);
        }

        public void ClearPlaylist()
        {
            _playlist.Clear();
        }

        public void PlayNew(MusicDetail2 music)
        {
            PlayById(music.Id);
        }

        private void PlayById(string musicId)
        {
            var music = _playlist.FirstOrDefault(x => x.Id == musicId);
            if (music == null)
            {
                return;
            }
            PlayByMusic(music);
        }

        private void PlayByMusic(MusicDetail2 music)
        {
            _player.Open(new Uri(music.PlayUrl));
            _player.Play();
            _currentMusicId = music.Id;
            IsPlaying = true;
            MusicStarted?.Invoke(music);
        }
        public void Play()
        {
            _player.Play();
            IsPlaying = true;
        }

        public void Pause()
        {
            _player.Pause();
            IsPlaying = false;
        }

        public void SetProgress(double percent)
        {
            var nd = _player.NaturalDuration;
            if (IsPlaying == false || nd.HasTimeSpan == false)
            {
                return;
            }
            double totalTime = nd.TimeSpan.TotalSeconds;
            var ts = TimeSpan.FromSeconds(totalTime * percent);
            _player.Position = ts;
        }

        public void Previous()
        {
            if (PlayMode == PlayModeEnum.RepeatOne)
            {
                PlayById(_currentMusicId);
                return;
            }
            if (PlayMode == PlayModeEnum.RepeatList)
            {
                int nextId = 0;
                for (int i = 0; i < _playlist.Count; i++)
                {
                    if (_playlist[i].Id == _currentMusicId)
                    {
                        nextId = i - 1;
                        break;
                    }
                }
                //列表第一首
                if (nextId < 0)
                {
                    nextId = _playlist.Count - 1;
                }

                PlayByMusic(_playlist[nextId]);
                return;
            }
            if (PlayMode == PlayModeEnum.Shuffle)
            {
                if (_playlist.Count <= 1)
                {
                    PlayById(_currentMusicId);
                    return;
                }

                MusicDetail2 randomMusic;
                do
                {
                    randomMusic = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<MusicDetail2>(_playlist);
                } while (randomMusic.Id == _currentMusicId);
                PlayById(randomMusic.Id);
            }
        }

        public void Next()
        {
            if (PlayMode == PlayModeEnum.RepeatOne)
            {
                PlayById(_currentMusicId);
                return;
            }
            if (PlayMode == PlayModeEnum.RepeatList)
            {
                int nextId = 0;
                for (int i = 0; i < _playlist.Count; i++)
                {
                    if (_playlist[i].Id == _currentMusicId)
                    {
                        nextId = i + 1;
                        break;
                    }
                }
                //列表最后一首
                if (_playlist.Count == nextId)
                {
                    nextId = 0;
                }

                PlayByMusic(_playlist[nextId]);
                return;
            }
            if (PlayMode == PlayModeEnum.Shuffle)
            {
                if (_playlist.Count <= 1)
                {
                    PlayById(_currentMusicId);
                    return;
                }

                MusicDetail2 randomMusic = null;
                do
                {
                    randomMusic = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<MusicDetail2>(_playlist);
                } while (randomMusic.Id == _currentMusicId);
                PlayById(randomMusic.Id);
            }
        }

        public (bool isPlaying, TimeSpan position, TimeSpan total, double percent) GetPosition()
        {
            var nd = _player.NaturalDuration;
            if (IsPlaying == false || nd.HasTimeSpan == false)
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
