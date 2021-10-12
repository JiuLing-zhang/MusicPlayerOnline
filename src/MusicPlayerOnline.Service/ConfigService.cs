using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class ConfigService : IConfigService
    {
        public async Task<GeneralConfig> ReadGeneralConfig()
        {
            return await DatabaseProvide.Database.Table<GeneralConfig>().FirstOrDefaultAsync();
        }

        public async Task WriteGeneralConfig(GeneralConfig generalConfig)
        {
            await DatabaseProvide.Database.UpdateAsync(generalConfig);
        }

        public async Task<PlayConfig> ReadPlayConfig()
        {
            return await DatabaseProvide.Database.Table<PlayConfig>().FirstOrDefaultAsync();
        }

        public async Task WritePlayConfig(PlayConfig playConfig)
        {
            await DatabaseProvide.Database.UpdateAsync(playConfig);
        }

        public async Task<PlayerConfig> ReadPlayerConfig()
        {
            return await DatabaseProvide.Database.Table<PlayerConfig>().FirstOrDefaultAsync();
        }

        public async Task WritePlayerConfig(PlayerConfig playerConfig)
        {
            await DatabaseProvide.Database.UpdateAsync(playerConfig);
        }
    }
}
