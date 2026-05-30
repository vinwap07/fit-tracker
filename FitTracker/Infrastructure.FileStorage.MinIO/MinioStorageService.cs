using Application.Abstractions;
using Domain.ValueObjects;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.FileService;

public class MinioStorageService(IMinioClient minioClient, IOptions<MinioOptions> minioOptions) : IFileStorageService
{
    private readonly string _defaultBucket = minioOptions.Value.BucketName;
    
    public async Task<Media> UploadFileAsync(Stream content, string fileName, string contentType, string? bucket, CancellationToken ct = default)
    {
        var targetBucket = string.IsNullOrWhiteSpace(bucket) ? _defaultBucket : bucket;
        
        var fileKey = $"{Guid.NewGuid()}_{fileName}";
        
        var beArgs = new BucketExistsArgs().WithBucket(targetBucket);
        if (!await minioClient.BucketExistsAsync(beArgs, ct))
        {
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(targetBucket), ct);
        }
        
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(targetBucket)
            .WithObject(fileKey)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType(contentType);
        
        await minioClient.PutObjectAsync(putObjectArgs, ct);
        
        return new Media(targetBucket, fileKey);
    }

    public async Task<string> GetPresignedUrlAsync(string bucket, string key, int expirationMinutes = 60)
    {
        var targetBucket = string.IsNullOrWhiteSpace(bucket) ? _defaultBucket : bucket;
        
        var args = new PresignedGetObjectArgs()
            .WithBucket(targetBucket)
            .WithObject(key)
            .WithExpiry(expirationMinutes * 60);

        return await minioClient.PresignedGetObjectAsync(args);
    }

    public async Task DeleteFileAsync(string bucket, string fileKey, CancellationToken ct)
    {
        var targetBucket = string.IsNullOrWhiteSpace(bucket) ? _defaultBucket : bucket;
        
        var args = new RemoveObjectArgs()
            .WithBucket(targetBucket)
            .WithObject(fileKey);
        
        await minioClient.RemoveObjectAsync(args, ct);
    }
}