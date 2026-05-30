using Domain.Exceptions;

namespace Domain.ValueObjects;

public class Media
{
    public string Bucket { get; init; }
    public string Key { get; init; }
    protected Media() { }

    public Media(string bucket, string key)
    {
        if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(key))
        {
            throw new DomainException("Bucket and key are required");
        }
        Bucket = bucket;
        Key = key;
    }
}