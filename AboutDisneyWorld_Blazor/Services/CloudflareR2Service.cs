using AboutDisneyWorld_Blazor.Models;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
namespace AboutDisneyWorld_Blazor.Services;
public class CloudflareR2Service
{
    private readonly AmazonS3Client s3Client;
    private readonly string bucketName = "disneyphotos";

    public CloudflareR2Service(IConfiguration config)
    {
        var accessKey = config["Cloudflare:AccessKey"];
        var secretKey = config["Cloudflare:SecretAccessKey"];
        var serviceUrl = config["Cloudflare:ServiceURL"];
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = serviceUrl
        });
    }

    public async Task PutObject(Photo photo)
    {
        // Convert your content or data into a stream
        var byteArray = photo.ImageData;
        using var memoryStream = new MemoryStream(byteArray);

        memoryStream.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = photo.FileName,
            ContentType = photo.ContentType,
            InputStream = memoryStream,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        var response = await s3Client.PutObjectAsync(request);

        Console.WriteLine("ETag: {0}", response.ETag);
    }
}