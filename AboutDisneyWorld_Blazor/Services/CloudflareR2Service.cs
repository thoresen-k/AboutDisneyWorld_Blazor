using AboutDisneyWorld_Blazor.Models;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Driver.Core.Operations;
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

    public async Task PutObject(IBrowserFile file)
    {
        // Convert your content or data into a stream
        using var stream = file.OpenReadStream(file.Size);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var data = ms.ToArray();
        using var memoryStream = new MemoryStream(data);

        memoryStream.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = file.Name,
            ContentType = file.ContentType,
            InputStream = memoryStream,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        var response = await s3Client.PutObjectAsync(request);

        Console.WriteLine("ETag: {0}", response.ETag);
    }

    public async Task DeleteObject(Photo photo)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = photo.FileName
        };

        var response = await s3Client.DeleteObjectAsync(request);
        Console.WriteLine($"[CloudflareR2] Delete request sent for key '{photo.FileName}'. HTTP Status: {response.HttpStatusCode}");
    }
}