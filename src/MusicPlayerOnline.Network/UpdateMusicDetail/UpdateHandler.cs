using System;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Network.UpdateMusicDetail
{
    public abstract class UpdateHandler
    {
        private UpdateHandler _nextHandler;
        private readonly PlatformEnum _platform;
        protected UpdateHandler(PlatformEnum platform)
        {
            _platform = platform;
        }
        public void SetNextHandler(UpdateHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public async Task<MusicDetail> Update(MusicDetail music)
        {
            if (music.Platform == _platform)
            {
                return await DoUpdate(music);
            }

            if (_nextHandler != null)
            {
                return await _nextHandler.Update(music);
            }

            throw new Exception("未找到对应的构建工具");
        }
        public abstract Task<MusicDetail> DoUpdate(MusicDetail music);
    }
}
