using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Model.Model
{
    public class MusicDetail : MusicBase
    {
        /// <summary>
        /// 平台
        /// </summary>
        public PlatformEnum Platform { get; set; }
        /// <summary>
        /// 对应平台的ID
        /// </summary>
        public string PlatformId { get; set; }
        /// <summary>
        /// 缓存地址
        /// </summary>
        public string CachePath { get; set; }
    }
}
