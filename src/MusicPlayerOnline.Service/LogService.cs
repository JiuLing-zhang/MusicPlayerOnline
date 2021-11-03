using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Log;

namespace MusicPlayerOnline.Service
{
    public class LogService : ILogService
    {
        public async Task<List<LogDetail>> GetLogs()
        {
            return await DatabaseProvide.DatabaseAsync.Table<LogDetail>().OrderByDescending(x => x.Timestamp).Take(200).ToListAsync();
        }

        public async Task ClearLogs()
        {
            await DatabaseProvide.DatabaseAsync.DeleteAllAsync<LogDetail>();
        }

        public Task UploadLogs(List<LogDetail> logs)
        {
            throw new NotImplementedException();
        }
    }
}
