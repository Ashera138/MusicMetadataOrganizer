using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater.UnitTests.MockFile
{
    internal class MockMetadataFile : IFile
    {
        public int MetadataFileId { get; set; }
        public string Filepath { get; set; }
        public int BitRate { get; set; }
        public string MediaType { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public string Genres { get; set; }
        public string Lyrics { get; set; }
        public long? TrackNo { get; set; }
        public long? Year { get; set; }
        public byte? Rating { get; set; }
        public long DurationInTicks { get; set; }

        public bool Equals(IFile otherFile)
        {
            bool isEqual = true;
            var metadataFile = otherFile as MockMetadataFile;
            if (Artist != metadataFile.Artist ||
                Album != metadataFile.Album ||
                TrackNo != metadataFile.TrackNo ||
                BitRate != metadataFile.BitRate ||
                DurationInTicks != metadataFile.DurationInTicks)
                isEqual = false;
            return isEqual;
        }

        public bool TrySave() => false;

        public override string ToString()
        {
            return $"{Artist} - {Title}";
        }
    }
}
