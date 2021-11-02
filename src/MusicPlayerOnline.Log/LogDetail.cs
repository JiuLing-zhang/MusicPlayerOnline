using System;
using SQLite;

namespace MusicPlayerOnline.Log
{
    public class LogDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Int64 Timestamp { get; set; }
        public LogTypeEnum LogType { get; set; }
        public string Message { get; set; }
    }
    public enum LogTypeEnum
    {
        消息 = 0,
        警告 = 1,
        错误 = 2
    }
}
