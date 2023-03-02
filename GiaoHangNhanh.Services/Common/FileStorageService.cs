using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Common
{
    public interface IFileStorageService
    {
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
        Task DeleteFilePathNotCombineAsync(string fileName);
    }
    public class FileStorageService : IFileStorageService
    {
        private readonly string _userContentFolder;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = webHostEnvironment.WebRootPath;
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = _userContentFolder + fileName;
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public async Task DeleteFilePathNotCombineAsync(string fileName)
        {
            if (File.Exists(_userContentFolder + fileName))
            {
                await Task.Run(() => File.Delete(_userContentFolder + fileName));
            }
        }
    }
}
