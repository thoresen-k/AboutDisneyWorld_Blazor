using Amazon.Runtime;
using Amazon.S3;

namespace AboutDisneyWorld_Blazor.Services;
public class CloudflareR2Service
{
    private readonly IAmazonS3 s3Client;
    private readonly string bucketName = "disneyphotos";
    private readonly string serviceUrl;

    public CloudflareR2Service(IConfiguration config)
    {
        var accessKey = config["Cloudflare:AccessKey"];
        var secretKey = config["Cloudflare:SecretAccessKey"];
        serviceUrl = config["Cloudflare:ServiceURL"];
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = serviceUrl
        });
    }
}