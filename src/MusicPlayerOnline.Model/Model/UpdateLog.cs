using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlayerOnline.Model.Model
{
    //TODO 几个项目中的日志模块的模型建立的有点问题，需要重构
    public class UpdateLog
    {
        public string SessionId { get; set; }
        public List<UpdateLogDetail> Logs { get; set; }
    }

    public class UpdateLogDetail
    {
        public Int64 Timestamp { get; set; }
        public int LogType { get; set; }
        public string Message { get; set; }
    }
}
