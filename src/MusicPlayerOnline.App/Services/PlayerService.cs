using System;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Common;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.Services
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
            await Logger.WriteAsync(LogTypeEnum.警告, $"播放服务-播放失败：内部错误");
            await Next();
        }

        public void Play(MusicDetail music)
        {
            PlayingMusic = music;
            _audio.Play(music.CachePath);
            MediaBegin?.Invoke();
            Logger.Write(LogTypeEnum.消息, $"播放服务-播放");
        }

        public void Start()
        {
            _audio.Start();
            Logger.Write(LogTypeEnum.消息, $"播放服务-开始");
        }
        public void Pause()
        {
            _audio.Pause();
            Logger.Write(LogTypeEnum.消息, $"播放服务-暂停");
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
            await Logger.WriteAsync(LogTypeEnum.消息, $"播放服务-上一首，模式：{GlobalArgs.AppConfig.Player.PlayMode.GetDescription()}");
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.RepeatOne)
            {
                Play(PlayingMusic);
                return;
            }

            var playlist = await _playlistService.GetList();
            if (playlist.Count == 0)
            {
                await Logger.WriteAsync(LogTypeEnum.警告, $"播放服务-播放列表为空");
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
            await Logger.WriteAsync(LogTypeEnum.消息, $"播放服务-下一首，模式：{GlobalArgs.AppConfig.Player.PlayMode.GetDescription()}");
            if (GlobalArgs.AppConfig.Player.PlayMode == PlayModeEnum.RepeatOne)
            {
                Play(PlayingMusic);
                return;
            }

            var playlist = await _playlistService.GetList();
            if (playlist.Count == 0)
            {
                await Logger.WriteAsync(LogTypeEnum.警告, $"播放服务-播放列表为空");
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
