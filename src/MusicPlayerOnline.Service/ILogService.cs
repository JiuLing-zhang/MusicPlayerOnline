using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPlayerOnline.Log;

namespace MusicPlayerOnline.Service
{
    public interface ILogService
    {
        Task<List<LogDetail>> GetLogs();

        Task ClearLogs();

    }
}
