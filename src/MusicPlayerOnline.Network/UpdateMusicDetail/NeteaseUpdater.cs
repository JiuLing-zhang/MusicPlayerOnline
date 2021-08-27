using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Network.MusicProvider;

namespace MusicPlayerOnline.Network.UpdateMusicDetail
{
    public class NeteaseUpdater : UpdateHandler
    {
        private readonly IMusicProvider _myMusicProvider;
        public NeteaseUpdater(PlatformEnum platform) : base(platform)
        {
            _myMusicProvider = new NeteaseMusicProvider();
        }

        public override async Task<MusicDetail> DoUpdate(MusicDetail music)
        {
            return await _myMusicProvider.UpdateMusicDetail(music);
        }
    }
}
