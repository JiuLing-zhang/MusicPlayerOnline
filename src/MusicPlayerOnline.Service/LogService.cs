using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Model;

namespace MusicPlayerOnline.Service
{
    public class LogService : ILogService
    {
        public async Task<List<LogDetail>> GetLogs()
        {
            return await DatabaseProvide.DatabaseAsync.Table<LogDetail>().OrderByDescending(x => x.Timestamp).Take(200).ToListAsync();
        }
    }
}
