using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IConfigService
    {
        GeneralConfig ReadGeneralConfig();
        Task<GeneralConfig> ReadGeneralConfigAsync();
        Task WriteGeneralConfigAsync(GeneralConfig generalConfig);

        PlatformConfig ReadPlatformConfig();
        Task<PlatformConfig> ReadPlatformConfigAsync();
        Task WritePlatformConfigAsync(PlatformConfig platformConfig);

        PlayConfig ReadPlayConfig();
        Task<PlayConfig> ReadPlayConfigAsync();
        Task WritePlayConfigAsync(PlayConfig playConfig);

        PlayerConfig ReadPlayerConfig();
        Task<PlayerConfig> ReadPlayerConfigAsync();
        Task WritePlayerConfigAsync(PlayerConfig playerConfig);
    }
}
