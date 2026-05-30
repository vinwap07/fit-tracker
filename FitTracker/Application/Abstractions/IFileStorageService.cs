using Domain.ValueObjects;

namespace Application.Abstractions;

public interface IFileStorageService
{
    Task<Media> UploadFileAsync(Stream content, string fileName, string contentType, string? bucket, CancellationToken ct);
    
    Task<string> GetPresignedUrlAsync(string bucket, string key, int expirationMinutes = 60);
    
    Task DeleteFileAsync(string bucketName, string fileKey, CancellationToken ct);
}