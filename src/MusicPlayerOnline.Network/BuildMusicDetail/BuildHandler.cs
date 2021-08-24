using System;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Network.BuildMusicDetail
{
    public abstract class BuildHandler
    {
        private BuildHandler _nextHandler;
        private readonly PlatformEnum _platform;
        protected BuildHandler(PlatformEnum platform)
        {
            _platform = platform;
        }
        public void SetNextHandler(BuildHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public async Task<MusicDetail2> Build(MusicSearchResult music)
        {
            if (music.Platform == _platform)
            {
                return await DoBuild(music);
            }

            if (_nextHandler != null)
            {
                return await _nextHandler.Build(music);
            }

            throw new Exception("未找到对应的构建工具");
        }
        public abstract Task<MusicDetail2> DoBuild(MusicSearchResult music);
    }
}
