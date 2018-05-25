using MusicMetadataUpdater.UnitTests.MockFile;
using System;
using System.Data;

namespace MetadataFileTests.Misc
{
    internal static class TestClassFactory
    {
        // duplicated code
        internal static MockMetadataFile GetMockMetadataFileWithDummyValues()
        {
            return new MockMetadataFile()
            {
                MetadataFileId = 1,
                Filepath = @"C:/Not a real filepath/file.mp3",
                BitRate = 250,
                MediaType = "Audio",
                Artist = "Artist",
                Album = "Album",
                Title = "Title",
                Genres = "Genres",
                Lyrics = "Lyrics",
                TrackNo = 1,
                Year = 2018,
                Rating = null,
                DurationInTicks = 2000
            };
        }

        // duplicated code
        internal static MockMetadataFile GetMockMetadataFileWithNoNullDummyValues()
        {
            return new MockMetadataFile()
            {
                MetadataFileId = 1,
                Filepath = @"C:/Not a real filepath/file.mp3",
                BitRate = 250,
                MediaType = "Audio",
                Artist = "Artist",
                Album = "Album",
                Title = "Title",
                Genres = "Genres",
                Lyrics = "Lyrics",
                TrackNo = 1,
                Year = 2018,
                Rating = 5,
                DurationInTicks = 2000
            };
        }

        internal static MockSystemFile GetMockSystemFileWithDummyValues()
        {
            return new MockSystemFile()
            {
                SystemFileId = 1,
                Filepath = @"C:/Not a real filepath/file.mp3",
                Name = "Test Name",
                Directory = @"C:/Not a real directory/",
                Extension = ".mp3",
                CreationTime = DateTime.Now,
                LastAccessTime = DateTime.Now,
                LengthInBytes = 1000
            };
        }

        internal static DataTable GetDataTable(MockMetadataFile file)
        {
            var table = new DataTable();
            table.Columns.Add("MetadataFileId");
            table.Columns.Add("Filepath");
            table.Columns.Add("BitRate");
            table.Columns.Add("MediaType");
            table.Columns.Add("Artist");
            table.Columns.Add("Album");
            table.Columns.Add("Title");
            table.Columns.Add("Genres");
            table.Columns.Add("Lyrics");
            table.Columns.Add("TrackNo");
            table.Columns.Add("Year");
            table.Columns.Add("Rating");
            table.Columns.Add("DurationInTicks");

            table.Rows.Add(file.MetadataFileId, file.Filepath, file.BitRate, file.MediaType, 
                           file.Artist, file.Album, file.Title, file.Genres, file.Lyrics, 
                           file.TrackNo, file.Year, file.Rating, file.DurationInTicks);
            return table;
        }

        internal static DataTable GetDataTable(MockSystemFile file)
        {
            var table = new DataTable();
            table.Columns.Add("SystemFileId");
            table.Columns.Add("Filepath");
            table.Columns.Add("Name");
            table.Columns.Add("Directory");
            table.Columns.Add("Extension");
            table.Columns.Add("CreationTime");
            table.Columns.Add("LastAccessTime");
            table.Columns.Add("LengthInBytes");

            table.Rows.Add(file.SystemFileId, file.Filepath, file.Name, file.Directory,
                           file.Extension, file.CreationTime, file.LastAccessTime, file.LengthInBytes);
            return table;
        }
    }
}
