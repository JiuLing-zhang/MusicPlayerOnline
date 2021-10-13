using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MusicPlayerOnline.Model.Model
{
    /// <summary>
    /// 播放列表
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// 歌曲ID
        /// </summary>
        [PrimaryKey]
        public string MusicDetailId { get; set; }

        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string Artist { get; set; }
    }
}
