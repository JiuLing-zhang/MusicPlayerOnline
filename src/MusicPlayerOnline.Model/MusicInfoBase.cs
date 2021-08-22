namespace MusicPlayerOnline.Model
{
    public class MusicInfoBase
    {
        public int Id { get; set; }
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
        /// 图片地址
        /// </summary>
        public string PicUrl { get; set; }
    }
}
