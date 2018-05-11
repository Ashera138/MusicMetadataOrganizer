namespace MusicMetadataUpdater_v2._0
{
    public struct GracenoteAPIResponse
    {
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public long? Year { get; set; }
        public string Genres { get; set; }
        public long? TrackNo { get; set; }
    }
}
