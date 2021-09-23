using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MusicPlayerOnline.Model.Model
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class MyFavorite
    {
        [PrimaryKey]
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 展示的图片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 歌曲总数
        /// </summary>
        public int MusicCount { get; set; }
    }
}
