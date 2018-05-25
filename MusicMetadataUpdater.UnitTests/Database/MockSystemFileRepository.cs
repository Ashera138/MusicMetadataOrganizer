using FizzWare.NBuilder;
using MusicMetadataUpdater.UnitTests.MockFile;
using MusicMetadataUpdater_v2._0;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataUpdater.UnitTests.Database
{
    internal class MockSystemFileRepository : MusicMetadataUI.Database.IFileRepository<MockSystemFile>
    {
        public DataTable DataTable = new DataTable();

        public void GenerateDataTable(int rows = 100)
        {
            DataTable = TableMaker.GenerateDataTable<MockSystemFile>(rows);
        }

        public async Task<bool> AddFileAsync(MockSystemFile file)
        {
            if (await ContainsAsync(file))
                return false;

            bool addSucceeded;

            try
            {
                DataRow row = DataTable.NewRow();
                row["SystemFileId"] = file.SystemFileId;
                row["Filepath"] = file.Filepath;
                row["Name"] = file.Name;
                row["Directory"] = file.Directory;
                row["Extension"] = file.Extension;
                row["CreationTime"] = file.CreationTime;
                row["LastAccessTime"] = file.LastAccessTime;
                row["LengthInBytes"] = file.LengthInBytes;

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

        public async Task<List<MockSystemFile>> GetFilesAsync()
        {
            return await DataTable.ToListAsync<MockSystemFile>();
        }

        public async Task<MockSystemFile> GetFileAsync(string filepath)
        {
            List<MockSystemFile> files = await DataTable.ToListAsync<MockSystemFile>();
            return files.Where(f => f.Filepath == filepath).FirstOrDefault();
        }

        public async Task<bool> UpdateFileAsync(MockSystemFile file)
        {
            bool updateSuccess = false;

            foreach (DataRow row in DataTable.Rows)
            {
                if (Convert.ToInt32(row["SystemFileId"]) == file.SystemFileId)
                {
                    row["Name"] = file.Name;
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

        public async Task<bool> ContainsAsync(MockSystemFile file)
        {
            bool containsFile = false;

            foreach (DataRow row in DataTable.Rows)
            {
                MockSystemFile rowAsMockFile = await Task.Run(() => row.ToObject<MockSystemFile>());
                if (rowAsMockFile.Equals(file))
                    containsFile = true;
            }
            return containsFile;
        }
    }
}
