using MusicMetadataUpdater_v2._0;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicMetadataUI.Database
{
    public interface IFileRepository<T> where T : IFile
    {
        Task<bool> AddFileAsync(T file);
        Task<List<T>> GetFilesAsync();
        Task<T> GetFileAsync(string filepath);
        Task<bool> UpdateFileAsync(T file);
        Task DeleteFileAsync(string filepath);
        Task<bool> ContainsAsync(T file);
    }
}
