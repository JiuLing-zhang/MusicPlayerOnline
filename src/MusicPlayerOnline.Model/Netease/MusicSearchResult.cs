namespace MusicPlayerOnline.Model.Netease
{
    public class MusicSearchResult
    {
        public Song[] songs { get; set; }
        public int songCount { get; set; }
    }

    public class Song
    {
        public string name { get; set; }
        public string[] alias { get; set; }
        public Artist[] artists { get; set; }
        public Album album { get; set; }
        public int duration { get; set; }

    }

    public class Album
    {
        public string name { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
    }
}
