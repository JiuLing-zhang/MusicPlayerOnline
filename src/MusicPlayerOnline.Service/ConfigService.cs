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

        public SearchConfig ReadPlatformConfig()
        {
            return DatabaseProvide.Database.Table<SearchConfig>().FirstOrDefault();
        }

        public async Task<SearchConfig> ReadPlatformConfigAsync()
        {
            return await DatabaseProvide.DatabaseAsync.Table<SearchConfig>().FirstOrDefaultAsync();
        }

        public async Task WritePlatformConfigAsync(SearchConfig searchConfig)
        {
            await DatabaseProvide.DatabaseAsync.UpdateAsync(searchConfig);
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
