﻿using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Model.Model
{
    /// <summary>
    /// 搜索到的结果
    /// </summary>
    public class MusicSearchResult : MusicBase
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
        /// 歌曲时长，格式为“分:秒”，例如：05:44
        /// </summary>
        public string DurationText { get; set; }
        /// <summary>
        /// 平台特有数据
        /// </summary>
        public object PlatformData { get; set; }
    }
}
