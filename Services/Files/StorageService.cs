using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using RapidRents.Web.Services;
using System;
using System.Configuration;
using System.IO;

public class StorageService : IStorageService
{
    private const string awsDirectoryName = @"___________";
    private const string awsBucketName = @"___________";

    private IAmazonS3 client = null;

    public StorageService()
    {
        string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        if (this.client == null)
        {
            this.client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USWest2);
        }
    }

    public bool UploadFile(string key, Stream stream)
    {
        TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            BucketName = awsDirectoryName,
            CannedACL = S3CannedACL.AuthenticatedRead,
            Key = key
        };

        TransferUtility fileTransferUtility = new TransferUtility(this.client);
        fileTransferUtility.Upload(uploadRequest);
        return true;
    }

    public bool DeleteFile(string key)
    {
        client.DeleteObject(new DeleteObjectRequest() { BucketName = awsBucketName, Key = key });
        return true;
    }

    public string GeneratePreSignedURL(string key, int expireInSeconds)
    {
        string urlString = string.Empty;
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = awsDirectoryName,
            Key = key,
            Expires = DateTime.Now.AddSeconds(expireInSeconds)
        };

        urlString = this.client.GetPreSignedURL(request);
        return urlString;
    }
}
