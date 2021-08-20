using System.Collections.Generic;
using System.Threading.Tasks;
using JiuLing.CommonLibs.Model;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Network.MusicProvider
{
    public interface IMusicProvider
    {
        Task<JsonResult<List<MusicSearchResultModel>>> Search(string keyword);

        Task<JsonResult<string>> GetMusicUrl(int id);
    }
}
