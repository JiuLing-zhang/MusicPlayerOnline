using System.Collections.Generic;
using System.Threading.Tasks;
using JiuLing.CommonLibs.Model;
using MusicPlayerOnline.Model;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public interface IMusicProvider
    {
        Task<JsonResult<List<MusicInfo>>> Search(string keyword);
    }
}
