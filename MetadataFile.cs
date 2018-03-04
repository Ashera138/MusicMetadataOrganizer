using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace MusicMetadataUpdater_v2._0
{
    public class MetadataFile : IFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey("SystemFile")]
        public int FileId { get; set; }
        public SystemFile SystemFile { get; set; }

        private string _filepath;
        public string Filepath
        {
            get
            {
                return _filepath;
            }
            set
            {
                if (!System.IO.File.Exists(value))
                    throw new IOException();
                _filepath = value;
            }
        }
        public int BitRate { get; set; }
        [Required]
        [StringLength(40)]
        public string MediaType { get; set; }
        [StringLength(100)]
        public string Artist { get; set; }
        [StringLength(100)]
        public string Album { get; set; }
        [StringLength(1024)]
        public string Genres { get; set; }
        [StringLength(1024)]
        public string Lyrics { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public uint Track { get; set; }
        public uint Year { get; set; }
        public byte Rating { get; set; }
        public long DurationInTicks { get; set; }
        public bool CheckForUpdates { get; set; }
        private TagLib.File TagLibFile { get; set; }

        // This will be called by the database via EF
        private MetadataFile()
        {
            CreateTagLibFile();
            PopulateFields();
        }

        public MetadataFile(string filepath)
        {
            this.Filepath = filepath;
            CreateTagLibFile();
            PopulateFields();
        }

        private void CreateTagLibFile()
        {
            try
            {
                var file = TagLib.File.Create(Filepath);
            }
            catch (Exception ex)
            {
                LogWriter.Write($"MetadataFile.CreateTagLibFile() - Could not create a TagLibFile object from " +
                    $"'{Filepath}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private void PopulateFields()
        {
            Filepath = TagLibFile.Name;
            BitRate = TagLibFile.Properties.AudioBitrate;
            MediaType = TagLibFile.Properties.MediaTypes.ToString();
            Artist = TagLibFile.Tag.FirstAlbumArtist ?? "Unknown";
            Album = TagLibFile.Tag.Album ?? "Uknown";
            Genres = GetGenresFromMetadata(TagLibFile);
            Lyrics = TagLibFile.Tag.Lyrics ?? string.Empty;
            Title = TagLibFile.Tag.Title ?? "Uknown";
            Track = TagLibFile.Tag.Track;
            Year = TagLibFile.Tag.Year;
            Rating = GetRatingFromMetadata(TagLibFile);
            DurationInTicks = TagLibFile.Properties.Duration.Ticks;
        }

        private string GetGenresFromMetadata(TagLib.File file)
        {
            string genres = string.Empty;
            for (int i = 0; i < file.Tag.Genres.Length; i++)
            {
                if (i < TagLibFile.Tag.Genres.Length - 1)
                    genres += TagLibFile.Tag.Genres[i] + ", ";
                else
                    genres += TagLibFile.Tag.Genres[i];
            }
            return genres;
        }

        private byte GetRatingFromMetadata(TagLib.File file)
        {
            const byte noRating = 0;
            byte rating;
            try
            {
                var tag = file.GetTag(TagLib.TagTypes.Id3v2);
                var frame = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "WindowsUser", true);
                rating= frame.Rating;
            }
            catch (Exception)
            {
                rating = noRating;
            }
            return rating;
        }

        public void UpdateMetadataWithAPIResult(RESPONSE gracenoteAPIResult)
        {
            // Re-add sanitization
            Artist = gracenoteAPIResult.ALBUM.ARTIST;
            Album = gracenoteAPIResult.ALBUM.TITLE;
            Title = gracenoteAPIResult.ALBUM.TRACK.TITLE;
            Track = Convert.ToUInt32(gracenoteAPIResult.ALBUM.TRACK.TRACK_NUM);
            Year = Convert.ToUInt32(gracenoteAPIResult.ALBUM.DATE);
            Genres = gracenoteAPIResult.ALBUM.GENRE;
        }

        public void Save()
        {
            LoadCurrentMetadataIntoTagLibFileField();
            SaveTagLibFile();
        }

        private void LoadCurrentMetadataIntoTagLibFileField()
        {
            LoadCurrentArtistIntoTagLibFileField();
            TagLibFile.Tag.Album = Album;
            TagLibFile.Tag.Genres = Genres.Split(',');
            TagLibFile.Tag.Title = Title;
            TagLibFile.Tag.Track = Track;
            TagLibFile.Tag.Year = Year;
        }

        private void LoadCurrentArtistIntoTagLibFileField()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TagLibFile.Tag.Artists = new string[] { Artist };
#pragma warning restore CS0618 // Type or member is obsolete
            TagLibFile.Tag.AlbumArtists = new string[] { Artist };
            TagLibFile.Tag.Performers = new string[] { Artist };
        }

        private void SaveTagLibFile()
        {
            try
            {
                TagLibFile.Save();
            }
            catch (Exception ex)
            {
                LogWriter.Write($"MetadataFile.SaveTagLibFile() - " +
                    $"Can not save taglib data to '{Filepath}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        public bool Equals(IFile file)
        {
            bool isEqual = true;
            var metadataFile = file as MetadataFile;
            if (Artist != metadataFile.Artist ||
                Album != metadataFile.Album ||
                Track != metadataFile.Track ||
                BitRate != metadataFile.BitRate ||
                DurationInTicks != metadataFile.DurationInTicks)
                isEqual = false;
            return isEqual;
        }
    }
}
