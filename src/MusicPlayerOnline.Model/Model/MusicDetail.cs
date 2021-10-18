using JiuLing.CommonLibs.ExtensionMethods;
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
        /// 平台名称
        /// </summary>
        public string PlatformName => Platform.GetDescription();
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
