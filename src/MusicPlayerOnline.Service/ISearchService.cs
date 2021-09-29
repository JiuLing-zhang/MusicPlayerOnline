using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Model.Enum;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface ISearchService
    {
        Task<List<MusicSearchResult>> Search(PlatformEnum platform, string keyword);
        Task SaveResultToPlaylist(MusicSearchResult music);
    }
}
