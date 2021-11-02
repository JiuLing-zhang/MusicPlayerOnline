using System;
using System.Threading.Tasks;
using MusicPlayerOnline.Data;

namespace MusicPlayerOnline.Log
{
    public class Logger
    {
        public static void Write(LogTypeEnum logType, string message)
        {
            try
            {
                var log = new LogDetail()
                {
                    Timestamp = JiuLing.CommonLibs.Text.TimestampUtils.GetLen13(),
                    LogType = logType,
                    Message = message
                };
                DatabaseProvide.Database.Insert(log);
            }
            catch (Exception ex)
            {
                //TODO 记录日志失败时，重试一波？
            }
        }

        public static async Task WriteAsync(LogTypeEnum logType, string message)
        {
            try
            {
                var log = new LogDetail()
                {
                    Timestamp = JiuLing.CommonLibs.Text.TimestampUtils.GetLen13(),
                    LogType = logType,
                    Message = message
                };
                await DatabaseProvide.DatabaseAsync.InsertAsync(log);
            }
            catch (Exception ex)
            {
                //TODO 记录日志失败时，重试一波？
            }
        }
    }
}
