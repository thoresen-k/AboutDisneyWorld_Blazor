using Amazon;
using Amazon.Runtime;
using Amazon.S3;

namespace AboutDisneyWorld_Blazor.Services;
public class CloudflareR2Service
{
    private readonly IAmazonS3 s3Client;

    public CloudflareR2Service(IConfiguration config)
    {
        var accessKey = config["Cloudflare:AccessKey"];
        var secretKey = config["Cloudflare:SecretAccessKey"];
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = config["Cloudflare:ServiceURL"]
        });
    }

    public async Task<List<string>> ListBucketsAsync()
    {
        var response = await s3Client.ListObjectsAsync("disneyphotos");
        return response.S3Objects.Select(b => b.BucketName).ToList();
    }
}