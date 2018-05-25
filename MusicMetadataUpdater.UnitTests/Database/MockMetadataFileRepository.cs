using MusicMetadataUI.Database;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MusicMetadataUpdater.UnitTests.Database
{
    internal class MockMetadataFileRepository : IFileRepository<MockMetadataFile>
    {
        public DataTable DataTable = new DataTable();

        public void GenerateDataTable(int rows = 100)
        {
            DataTable = TableMaker.GenerateDataTable<MockMetadataFile>(rows);
        }

        public async Task<bool> AddFileAsync(MockMetadataFile file)
        {
            if (await ContainsAsync(file))
                return false;

            bool addSucceeded;

            try
            {
                DataRow row = DataTable.NewRow();
                row["MetadataFileId"] = file.MetadataFileId;
                row["Filepath"] = file.Filepath;
                row["BitRate"] = file.BitRate;
                row["MediaType"] = file.MediaType;
                row["Artist"] = file.Artist;
                row["Album"] = file.Album;
                row["Title"] = file.Title;
                row["Genres"] = file.Genres;
                row["Lyrics"] = file.Lyrics;
                row["TrackNo"] = file.TrackNo;
                row["Year"] = file.Year;
                row["Rating"] = file.Rating;
                row["DurationInTicks"] = file.DurationInTicks;

                DataTable.Rows.Add(row);
                await Task.Run(() => DataTable.AcceptChanges());
                addSucceeded = true;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"MetadataFileRepository.AddFileAsync() - Could not add the given file to the database. " +
                        $"File: {file}. {ex.GetType()}: \"{ex.Message}\"");
                    addSucceeded = false;
            }

            return addSucceeded;
        }

        public async Task<List<MockMetadataFile>> GetFilesAsync()
        {
            return await DataTable.ToListAsync<MockMetadataFile>();
        }

        public async Task<MockMetadataFile> GetFileAsync(string filepath)
        {
            List<MockMetadataFile> files = await DataTable.ToListAsync<MockMetadataFile>();
            return files.Where(f => f.Filepath == filepath).FirstOrDefault();
        }

        public async Task<bool> UpdateFileAsync(MockMetadataFile file)
        {
            bool updateSuccess = false;
             
            foreach (DataRow row in DataTable.Rows)
            {
                if (Convert.ToInt32(row["MetadataFileId"]) == file.MetadataFileId)
                {
                    row["Artist"] = file.Artist;
                    await Task.Run(() => DataTable.AcceptChanges());
                    updateSuccess = true;
                }
            }
            
            return updateSuccess;
        }

        public async Task DeleteFileAsync(string filepath)
        {
            foreach (DataRow row in DataTable.Rows)
            {
                if (row["Filepath"].ToString() == filepath)
                {
                    row.Delete();
                    break;
                }
            }
            await Task.Run(() => DataTable.AcceptChanges());
        }

        public async Task<bool> ContainsAsync(MockMetadataFile file)
        {
            bool containsFile = false;

            foreach (DataRow row in DataTable.Rows)
            {
                MockMetadataFile rowAsMockFile = await Task.Run(() => row.ToObject<MockMetadataFile>());
                if (rowAsMockFile.Equals(file))
                    containsFile = true;
            }
            return containsFile;
        }
    }
}
