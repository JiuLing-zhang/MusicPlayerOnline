using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IConfigService
    {
        GeneralConfig ReadGeneralConfig();
        Task<GeneralConfig> ReadGeneralConfigAsync();
        Task WriteGeneralConfigAsync(GeneralConfig generalConfig);

        SearchConfig ReadPlatformConfig();
        Task<SearchConfig> ReadPlatformConfigAsync();
        Task WritePlatformConfigAsync(SearchConfig searchConfig);

        PlayConfig ReadPlayConfig();
        Task<PlayConfig> ReadPlayConfigAsync();
        Task WritePlayConfigAsync(PlayConfig playConfig);

        PlayerConfig ReadPlayerConfig();
        Task<PlayerConfig> ReadPlayerConfigAsync();
        Task WritePlayerConfigAsync(PlayerConfig playerConfig);
    }
}
