using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class ConfigService : IConfigService
    {
        public GeneralConfig ReadGeneralConfig()
        {
            return DatabaseProvide.Database.Table<GeneralConfig>().FirstOrDefault();
        }

        public async Task<GeneralConfig> ReadGeneralConfigAsync()
        {
            return await DatabaseProvide.DatabaseAsync.Table<GeneralConfig>().FirstOrDefaultAsync();
        }

        public async Task WriteGeneralConfigAsync(GeneralConfig generalConfig)
        {
            await DatabaseProvide.DatabaseAsync.UpdateAsync(generalConfig);
        }

        public PlatformConfig ReadPlatformConfig()
        {
            return DatabaseProvide.Database.Table<PlatformConfig>().FirstOrDefault();
        }

        public async Task<PlatformConfig> ReadPlatformConfigAsync()
        {
            return await DatabaseProvide.DatabaseAsync.Table<PlatformConfig>().FirstOrDefaultAsync();
        }

        public async Task WritePlatformConfigAsync(PlatformConfig platformConfig)
        {
            await DatabaseProvide.DatabaseAsync.UpdateAsync(platformConfig);
        }

        public PlayConfig ReadPlayConfig()
        {
            return DatabaseProvide.Database.Table<PlayConfig>().FirstOrDefault();
        }

        public async Task<PlayConfig> ReadPlayConfigAsync()
        {
            return await DatabaseProvide.DatabaseAsync.Table<PlayConfig>().FirstOrDefaultAsync();
        }

        public async Task WritePlayConfigAsync(PlayConfig playConfig)
        {
            await DatabaseProvide.DatabaseAsync.UpdateAsync(playConfig);
        }

        public PlayerConfig ReadPlayerConfig()
        {
            return DatabaseProvide.Database.Table<PlayerConfig>().FirstOrDefault();
        }

        public async Task<PlayerConfig> ReadPlayerConfigAsync()
        {
            return await DatabaseProvide.DatabaseAsync.Table<PlayerConfig>().FirstOrDefaultAsync();
        }

        public async Task WritePlayerConfigAsync(PlayerConfig playerConfig)
        {
            await DatabaseProvide.DatabaseAsync.UpdateAsync(playerConfig);
        }
    }
}
