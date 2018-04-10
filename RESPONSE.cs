namespace MusicMetadataUpdater_v2._0
{
    public class RESPONSE
    {
        public ALBUM ALBUM { get; set; }
    }

    public class ALBUM
    {
        public string GN_ID { get; set; }
        public string ARTIST { get; set; }
        public string TITLE { get; set; }
        public string PKG_LANG { get; set; }
        public string DATE { get; set; }
        public string GENRE { get; set; }
        public int MATCHED_TRACK_NUM { get; set; }
        public int TRACK_COUNT { get; set; }
        public TRACK TRACK { get; set; }
    }

    public class TRACK
    {
        public int TRACK_NUM { get; set; }
        public string GN_ID { get; set; }
        public string TITLE { get; set; }
    }
}
