using Microsoft.Extensions.Configuration;
using Sneakers.Shop.Backend.Domain.Interfaces;

namespace Sneakers.Shop.Backend.Infrastructure.Storage
{
    public class StorageService : IStorageService
    {
        private readonly string _basePath;
        private readonly string _baseUrl;
        public StorageService(IConfiguration configuration)
        {
            _basePath = configuration["Storage:LocalPath"] ?? "wwwroot/uploads";
            _baseUrl = configuration["Storage:BaseUrl"] ?? "http://localhost:5000/uploads";
        }

        public async Task<string> UploadAsync(
            Stream fileStream, 
            string fileName,
            string folder,
            CancellationToken ct = default)
        {
            var folderPath = Path.Combine(_basePath, folder);
            Directory.CreateDirectory(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(folderPath, uniqueFileName);
            
            using var fileStreamOutput = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(fileStreamOutput, ct);

            return $"{_baseUrl}/{folder}/{uniqueFileName}";
        }

        public async Task<IEnumerable<string>> UploadManyAsync(
            IEnumerable<(Stream Stream, string FileName)> files,
            string folder,
            CancellationToken ct = default)
        {
            var urls = new List<string>();
            foreach (var (stream, fileName) in files)
            {
                var url = await UploadAsync(stream, fileName, folder, ct);
                urls.Add(url);
            }
            return urls;
        }

        public Task Delete(string fileUrl, CancellationToken ct = default)
        {
            var relativePath = fileUrl.Replace(_baseUrl, _basePath);
            if (File.Exists(relativePath))
                File.Delete(relativePath);
            return Task.CompletedTask;
        }

        public async Task<string> UpdateAsync(
            Stream fileStream,
            string oldFileUrl,
            string fileName,
            CancellationToken ct = default)
        {
            await Delete(oldFileUrl, ct);
            var folder = Path.GetDirectoryName(oldFileUrl.Replace(_baseUrl + "/", "")) ?? string.Empty;
            return await UploadAsync(fileStream, fileName, folder, ct);
        }
    }
}