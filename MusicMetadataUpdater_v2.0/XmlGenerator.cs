namespace MusicMetadataUpdater_v2._0
{
    internal static class XmlGenerator
    {
        private static readonly string _clientId;
        private static readonly string _userId;

        static XmlGenerator()
        {
            var connectionCredentials = SensitiveDataReader.GetConnectionCredentials();
            _clientId = connectionCredentials["clientId"];
            _userId = connectionCredentials["userId"];
        }

        internal static string CreateXmlToPost(MetadataFile metadataFile, bool includeAlbumInQuery = true)
        {
            var artist = metadataFile.Artist;
            var title = metadataFile.Title;
            var album = metadataFile.Album;
            return includeAlbumInQuery ? CreateRequest(artist, title, album) : CreateRequest(artist, title);
        }

        private static string CreateRequest(string artist, string title, string album)
        {
            var validArtist = System.Security.SecurityElement.Escape(artist);
            var validTitle = System.Security.SecurityElement.Escape(title);
            var validAlbum = System.Security.SecurityElement.Escape(album);

            return $"<QUERIES><AUTH><CLIENT>{_clientId}</CLIENT><USER>{_userId}</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{validArtist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\">{validAlbum}</TEXT><TEXT TYPE=\"TRACK_TITLE\">{validTitle}</TEXT></QUERY></QUERIES>";
        }

        private static string CreateRequest(string artist, string title)
        {
            var validArtist = System.Security.SecurityElement.Escape(artist);
            var validTitle = System.Security.SecurityElement.Escape(title);

            return $"<QUERIES><AUTH><CLIENT>{_clientId}</CLIENT><USER>{_userId}</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{validArtist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\"></TEXT><TEXT TYPE=\"TRACK_TITLE\">{validTitle}</TEXT></QUERY></QUERIES>";
        }
    }
}
