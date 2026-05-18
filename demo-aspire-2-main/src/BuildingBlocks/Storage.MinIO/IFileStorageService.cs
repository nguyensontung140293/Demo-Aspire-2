namespace BuildingBlocks.Storage.MinIO;

public interface IFileStorageService
{
    Task<string> UploadAsync(string bucketName, string objectName, Stream data, string contentType, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string bucketName, string objectName, CancellationToken ct = default);
    Task DeleteAsync(string bucketName, string objectName, CancellationToken ct = default);
    Task<string> GetPresignedUrlAsync(string bucketName, string objectName, int expirySeconds = 3600);
}
