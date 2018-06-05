using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MusicMetadataUpdater_v2._0;

namespace MusicMetadataUI.Database
{
    public class FileRepository : IFileRepository<SystemFile>
    {
        private FileDb _dbContext = new FileDb();

        public async Task<bool> AddFileAsync(SystemFile file)
        {
            if (await this.ContainsAsync(file))
                return false;

            bool addSucceeded;
            try
            {
                _dbContext.SystemFiles.Add(file);
                _dbContext.MetadataFiles.Add(file.MetadataFile);
                await _dbContext.SaveChangesAsync();
                addSucceeded = true;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"FileRepository.AddFileAsync() - Could not add the given file to the database. " +
                    $"File: {file}. {ex.GetType()}: \"{ex.Message}\"");
                addSucceeded = false;
            }
            return addSucceeded;
        }

        public Task<List<SystemFile>> GetFilesAsync()
        {
            return _dbContext.SystemFiles.ToListAsync();
        }

        public Task<List<MetadataFile>> GetMetadataFilesAsync()
        {
            return _dbContext.MetadataFiles.ToListAsync();
        }

        public Task<SystemFile> GetFileAsync(string filepath)
        {
            return _dbContext.SystemFiles.FirstOrDefaultAsync(f => f.Filepath == filepath);
        }

        public async Task<bool> UpdateFileAsync(SystemFile file)
        {
            var record = _dbContext.SystemFiles.SingleOrDefault(f => f.Filepath == file.Filepath);
            if (record == null)
                return false;

            _dbContext.Entry(record).CurrentValues.SetValues(file);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // ON CASCADE DELETE is enabled, therefore deletion is only required on the parent table.
        public async Task DeleteFileAsync(string filepath)
        {
            var metadataFile = _dbContext.SystemFiles.FirstOrDefault(f => f.Filepath == filepath);
            if (metadataFile != null)
            {
                _dbContext.SystemFiles.Remove(metadataFile);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<bool> ContainsAsync(SystemFile file)
        {
            return Task.Run(() => _dbContext.SystemFiles.AsEnumerable().Contains(file));
        }
    }
}
