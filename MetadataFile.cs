using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace MusicMetadataUpdater_v2._0
{
    public class MetadataFile : IFile
    {
        [ForeignKey("SystemFile")]
        public int MetadataFileId { get; set; }
        public virtual SystemFile SystemFile { get; set; }
        private string _filepath;
        [StringLength(260)]
        public string Filepath
        {
            get
            {
                return _filepath;
            }
            set
            {
                if (!File.Exists(value))
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
        // 4000 is maximum. 8000 gets converted to nvarchar(MAX)
        [StringLength(8000)]
        public string Lyrics { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public long TrackNo { get; set; }
        public long Year { get; set; }
        public byte Rating { get; set; }
        public long DurationInTicks { get; set; }

        [NotMapped]
        public bool CheckForUpdates { get; set; }
        [NotMapped]
        public GracenoteAPIResponse Response { get; set; }
        [NotMapped]
        public Dictionary<string, string> fieldsToBeUpdated = new Dictionary<string, string>();

        private TagLib.File TagLibFile { get; set; }

        private MetadataFile()
        {
            CreateTagLibFile();
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
                TagLibFile = TagLib.File.Create(Filepath);
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
            TrackNo = TagLibFile.Tag.Track;
            Year = TagLibFile.Tag.Year;
            Rating = GetRatingFromMetadata(TagLibFile);
            DurationInTicks = TagLibFile.Properties.Duration.Ticks;
            CheckForUpdates = true;
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
                rating = frame.Rating;
            }
            catch (Exception)
            {
                rating = noRating;
            }
            return rating;
        }

        public void UpdateMetadataWithAPIResult(GracenoteAPIResponse gracenoteResponse)
        {
            // Re-add sanitization
            Artist = gracenoteResponse.Artist;
            Album = gracenoteResponse.Album;
            Title = gracenoteResponse.Title;
            TrackNo = gracenoteResponse.TrackNo;
            Year = gracenoteResponse.Year;
            Genres = gracenoteResponse.Genres;
        }

        public bool TrySave()
        {
            bool success;
            try
            {
                LoadCurrentMetadataIntoTagLibFileField();
                TagLibFile.Save();
                success = true;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"MetadataFile.TrySave() - " +
                    $"Can not save TagLib data to '{Filepath}'. {ex.GetType()}: \"{ex.Message}\"");
                success = false;
            }
            return success;
        }

        private void LoadCurrentMetadataIntoTagLibFileField()
        {
            LoadCurrentArtistIntoTagLibFileField();
            TagLibFile.Tag.Album = Album;
            TagLibFile.Tag.Genres = Genres.Split(',');
            TagLibFile.Tag.Title = Title;
            TagLibFile.Tag.Track = Convert.ToUInt32(TrackNo);
            TagLibFile.Tag.Year = Convert.ToUInt32(Year);
        }

        private void LoadCurrentArtistIntoTagLibFileField()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TagLibFile.Tag.Artists = new string[] { Artist };
#pragma warning restore CS0618 // Type or member is obsolete
            TagLibFile.Tag.AlbumArtists = new string[] { Artist };
            TagLibFile.Tag.Performers = new string[] { Artist };
        }

        public bool Equals(IFile file)
        {
            bool isEqual = true;
            var metadataFile = file as MetadataFile;
            if (Artist != metadataFile.Artist ||
                Album != metadataFile.Album ||
                TrackNo != metadataFile.TrackNo ||
                BitRate != metadataFile.BitRate ||
                DurationInTicks != metadataFile.DurationInTicks)
                isEqual = false;
            return isEqual;
        }

        // Refactor
        public bool IsUpdateNeeded()
        {
            bool updateNeeded = false;
            if (Response.Artist != Artist)
            {
                fieldsToBeUpdated.Add("Artist", Response.Artist);
                updateNeeded = true;
            }
            if (Response.Album != Album)
            {
                fieldsToBeUpdated.Add("Album", Response.Album);
                updateNeeded = true;
            }
            if (Response.Title != Title)
            {
                fieldsToBeUpdated.Add("Title", Response.Title);
                updateNeeded = true;
            }
            if (Response.Year != Year)
            {
                fieldsToBeUpdated.Add("Year", Response.Year.ToString());
                updateNeeded = true;
            }
            if (Response.Genres != Genres)
            {
                fieldsToBeUpdated.Add("Genres", Response.Genres);
                updateNeeded = true;
            }
            if (Response.TrackNo != TrackNo)
            {
                fieldsToBeUpdated.Add("TrackNo", Response.TrackNo.ToString());
                updateNeeded = true;
            }
            return updateNeeded;
        }

        public override string ToString()
        {
            return $"{Artist} - {Title}";
        }
    }
}
