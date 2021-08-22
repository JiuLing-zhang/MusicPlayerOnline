using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Model.Model
{
    public class MusicSearchResultModel : MusicInfoBase
    {
        /// <summary>
        /// 平台
        /// </summary>
        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 歌曲时长，格式为“分:秒”，例如：05:44
        /// </summary>
        public string DurationText { get; set; }
        /// <summary>
        /// 费用类型
        /// </summary>
        public int Fee { get; set; }
    }
}
