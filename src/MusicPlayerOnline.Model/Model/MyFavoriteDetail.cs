using System;
using System.Collections.Generic;
using System.Text;
using MusicPlayerOnline.Model.Enum;
using SQLite;

namespace MusicPlayerOnline.Model.Model
{
    /// <summary>
    /// 收藏的明细
    /// </summary>
    public class MyFavoriteDetail
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string MyFavoriteId { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public PlatformEnum Platform { get; set; }
        public string MusicId { get; set; }
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string Album { get; set; }
    }
}
