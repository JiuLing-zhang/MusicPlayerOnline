using System;
using System.Collections.Generic;
using System.Text;
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
        public string MusicId { get; set; }
    }
}
