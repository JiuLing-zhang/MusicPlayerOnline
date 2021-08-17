namespace MusicPlayerOnline.Model
{
    public class MusicInfo
    {
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 艺人名称
        /// </summary>
        public string ArtistName { get; set; }
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string AlbumName { get; set; }
        /// <summary>
        /// 歌曲时长（毫秒）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 歌曲时长，格式为“分:秒”，例如：05:44
        /// </summary>
        public string DurationString { get; set; }
    }
}
