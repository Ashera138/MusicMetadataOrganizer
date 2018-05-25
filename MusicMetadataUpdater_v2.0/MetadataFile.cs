using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.CompilerServices;

namespace MusicMetadataUpdater_v2._0
{
    public class MetadataFile : IFile, INotifyPropertyChanged
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
                    throw new FileNotFoundException();
                _filepath = value;
                RaisePropertyChanged();
            }
        }

        public int BitRate { get; set; }

        [Required]
        [StringLength(40)]
        public string MediaType { get; set; }

        private string _artist;
        [StringLength(100)]
        public string Artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                RaisePropertyChanged();
            }
        }

        private string _album;
        [StringLength(100)]
        public string Album
        {
            get { return _album; }
            set
            {
                _album = value;
                RaisePropertyChanged();
            }
        }

        private string _title;
        [StringLength(100)]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        private string _genres;
        [StringLength(1024)]
        public string Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                RaisePropertyChanged();
            }
        }

        public string Lyrics { get; set; }

        private long? _trackNo;
        public long? TrackNo
        {
            get { return _trackNo; }
            set
            {
                _trackNo = value;
                RaisePropertyChanged();
            }
        }

        private long? _year;
        public long? Year
        {
            get { return _year; }
            set
            {
                _year = value;
                RaisePropertyChanged();
            }
        }

        public byte? Rating { get; set; }

        public long DurationInTicks { get; set; }

        [NotMapped]
        public bool CheckForUpdates { get; set; }

        [NotMapped]
        public GracenoteAPIResponse Response { get; set; }

        [NotMapped]
        [Obsolete("Going to be using the List<MetadataUpdate> instead")]
        public Dictionary<string, string> fieldsToBeUpdated = new Dictionary<string, string>();

        [NotMapped]
        public ObservableCollection<MetadataUpdate> Updates = new ObservableCollection<MetadataUpdate>();

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private TagLib.File TagLibFile { get; set; }

        // test field
        [NotMapped]
        public string Name
        {
            get { return $"{Artist} - {Title}"; }
        }

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

        // TODO: Revisit which fields to include in equality check
        public bool Equals(IFile otherFile)
        {
            bool isEqual = true;
            var metadataFile = otherFile as MetadataFile;
            if (Artist != metadataFile.Artist ||
                Album != metadataFile.Album ||
                TrackNo != metadataFile.TrackNo ||
                BitRate != metadataFile.BitRate ||
                DurationInTicks != metadataFile.DurationInTicks)
                isEqual = false;
            return isEqual;
        }

        public bool IsUpdateNeeded()
        {
            bool updateNeeded = false;
            if (Response.Artist != Artist ||
                    Response.Album != Album || 
                    Response.Title != Title || 
                    Response.Year != Year ||
                    Response.Genres != Genres || 
                    Response.TrackNo != TrackNo)
                updateNeeded = true;
            return updateNeeded;
        }

        public void PopulateUpdateList()
        {
            if (Response.Artist != Artist)
            {
                Updates.Add(new MetadataUpdate(MetadataFileId, "Artist", Artist, Response.Artist));
            }
            if (Response.Album != Album)
            {
                Updates.Add(new MetadataUpdate(MetadataFileId, "Album", Album, Response.Album));
            }
            if (Response.Title != Title)
            {
                Updates.Add(new MetadataUpdate(MetadataFileId, "Title", Title, Response.Title));
            }
            if (Response.Year != Year)
            {
                Updates.Add(new MetadataUpdate(MetadataFileId, "Year", Year.ToString(), Response.Year.ToString()));
            }
            if (Response.Genres != Genres)
            {
                Updates.Add(new MetadataUpdate(MetadataFileId, "Genres", Genres, Response.Genres));
            }
            if (Response.TrackNo != TrackNo)
            {
                Updates.Add(new MetadataUpdate(MetadataFileId, "TrackNo", TrackNo.ToString(), Response.TrackNo.ToString()));
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public override string ToString()
        {
            return $"{Artist} - {Title}";
        }
    }
}
