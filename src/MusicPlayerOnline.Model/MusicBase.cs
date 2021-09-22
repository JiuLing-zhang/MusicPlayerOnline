using SQLite;

namespace MusicPlayerOnline.Model
{
    public class MusicBase
    {
        /// <summary>
        /// ID，系统中唯一，guid.ToString("N")
        /// </summary>
        [PrimaryKey]
        public string Id { get; set; }
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// 歌曲时长（毫秒）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 播放地址
        /// </summary>
        public string PlayUrl { get; set; }
    }
}
