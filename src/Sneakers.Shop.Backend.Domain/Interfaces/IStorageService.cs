namespace Sneakers.Shop.Backend.Domain.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string folder, CancellationToken ct = default);
        Task<IEnumerable<string>> UploadManyAsync(IEnumerable<(Stream Stream, string FileName)> files, string folder, CancellationToken ct = default);
        Task Delete(string fileUrl, CancellationToken ct = default);
        Task<string> UpdateAsync(Stream fileStream, string oldFileUrl, string fileName, CancellationToken ct = default);
    }
}
