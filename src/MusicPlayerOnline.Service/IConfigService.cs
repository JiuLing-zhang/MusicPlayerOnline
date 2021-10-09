using System.Threading.Tasks;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public interface IConfigService
    {
        Task<GeneralConfig> ReadGeneralConfig();
        Task WriteGeneralConfig(GeneralConfig generalConfig);

        Task<PlayConfig> ReadPlayConfig();
        Task WritePlayConfig(PlayConfig playConfig);

        Task<PlayerConfig> ReadPlayerConfig();
        Task WritePlayerConfig(PlayerConfig playerConfig);
    }
}
