using System;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.Services
{
    public class PlayerService
    {
        private static IAudio _audio;
        private readonly IMusicService _musicService;
        private readonly IPlaylistService _playlistService;

        public Action MediaBegin;
        private static readonly PlayerService MyPlayerService = new PlayerService();
        public static PlayerService Instance()
        {
            return MyPlayerService;
        }
        public PlayerService()
        {
            _audio = DependencyService.Get<IAudio>();
            _audio.MediaBegin += Audio_MediaBegin;
            _audio.MediaEnded += Audio_MediaEnded;
            _audio.MediaFailed += Audio_MediaFailed;

            _musicService = new MusicService();
            _playlistService = new PlaylistService();
        }
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying => _audio.IsPlaying;
        /// <summary>
        /// 正在播放的歌曲信息
        /// </summary>
        public MusicDetail PlayingMusic;

        private void Audio_MediaBegin()
        {

        }

        private async void Audio_MediaEnded()
        {
            await Next();
        }
        private async void Audio_MediaFailed()
        {
            DependencyService.Get<IToast>().Show("播放失败，准备跳到下一首");
            await Next();
        }

        public void Play(MusicDetail music)
        {
            PlayingMusic = music;
            _audio.Play(music.CachePath);
            MediaBegin?.Invoke();
        }

        public void Start()
        {
            _audio.Start();
        }
        public void Pause()
        {
            _audio.Pause();
        }

        public (int Duration, int Position) GetPosition()
        {
            return _audio.GetPosition();
        }

        /// <summary>
        /// 上一首
        /// </summary>
        public async Task Previous()
        {
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.RepeatOne)
            {
                Play(PlayingMusic);
                return;
            }

            var playlist = await _playlistService.GetList();
            if (playlist.Count == 0)
            {
                return;
            }

            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.RepeatList)
            {
                int nextId = 0;
                for (int i = 0; i < playlist.Count; i++)
                {
                    if (playlist[i].MusicDetailId == PlayingMusic.Id)
                    {
                        nextId = i - 1;
                        break;
                    }
                }
                //列表第一首
                if (nextId < 0)
                {
                    nextId = playlist.Count - 1;
                }

                var music = await _musicService.GetMusicDetail(playlist[nextId].MusicDetailId);
                Play(music);
                return;
            }
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.Shuffle)
            {
                if (playlist.Count <= 1)
                {
                    Play(PlayingMusic);
                    return;
                }

                string randomMusicId;
                do
                {
                    randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicDetailId;
                } while (randomMusicId == PlayingMusic.Id);
                var music = await _musicService.GetMusicDetail(randomMusicId);
                Play(music);
            }
        }

        /// <summary>
        /// 下一首
        /// </summary>
        public async Task Next()
        {
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.RepeatOne)
            {
                Play(PlayingMusic);
                return;
            }

            var playlist = await _playlistService.GetList();
            if (playlist.Count == 0)
            {
                return;
            }
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.RepeatList)
            {
                int nextId = 0;
                for (int i = 0; i < playlist.Count; i++)
                {
                    if (playlist[i].MusicDetailId == PlayingMusic.Id)
                    {
                        nextId = i + 1;
                        break;
                    }
                }
                //列表最后一首
                if (playlist.Count == nextId)
                {
                    nextId = 0;
                }

                var music = await _musicService.GetMusicDetail(playlist[nextId].MusicDetailId);
                Play(music);
                return;
            }
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.Shuffle)
            {
                if (playlist.Count <= 1)
                {
                    Play(PlayingMusic);
                    return;
                }

                string randomMusicId;
                do
                {
                    randomMusicId = JiuLing.CommonLibs.Random.RandomUtils.GetOneFromList<Playlist>(playlist).MusicDetailId;
                } while (randomMusicId == PlayingMusic.Id);
                var music = await _musicService.GetMusicDetail(randomMusicId);
                Play(music);
            }
        }
    }
}
