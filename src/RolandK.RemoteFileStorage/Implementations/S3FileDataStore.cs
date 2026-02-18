using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using RolandK.RemoteFileStorage.Util;

namespace RolandK.RemoteFileStorage.Implementations;

class S3FileDataStore : IFileDataStore
{
    private readonly FileDataStoreOptions _options;
    
    public S3FileDataStore(FileDataStoreOptions options)
    {
        _options = options;
    }

    public async Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken)
    {
        var s3Config = ConfigureS3Client();
        var s3Client =  ConfigureS3Client(s3Config);
        
        try
        {
            await s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest
            {
                BucketName = _options.S3BucketName,
                Key = filePath
            }, cancellationToken);

            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var s3Config = ConfigureS3Client();
        var s3Client = ConfigureS3Client(s3Config);

        TransferUtility? transferUtility = null;
        Stream? resultStream = null;
        try
        {
            transferUtility = new TransferUtility(s3Client);
            resultStream = await transferUtility.OpenStreamAsync(_options.S3BucketName, filePath, cancellationToken);
            
            return new StreamWithDisposables(
                resultStream,
                transferUtility);
        }
        catch
        {
            if (resultStream != null) { await resultStream.DisposeAsync(); }
            if (transferUtility != null) { transferUtility.Dispose(); }

            throw;
        }
    }

    public Task<IUploadUtility> UploadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var s3Config = ConfigureS3Client();

        return Task.FromResult((IUploadUtility)new S3CachedUploadUtility(async (streamToUpload, innerCancellationToken) =>
        {
            var s3Client = ConfigureS3Client(s3Config);
            
            TransferUtility? transferUtility = null;
            Stream? resultStream = null;
            try
            {
                transferUtility = new TransferUtility(s3Client);
   
                await transferUtility.UploadAsync(
                    streamToUpload,
                    _options.S3BucketName,
                    filePath,
                    innerCancellationToken);
            }
            catch
            {
                if (resultStream != null) { await resultStream.DisposeAsync(); }
                if (transferUtility != null) { transferUtility.Dispose(); }

                throw;
            }
        }));
    }

    private AmazonS3Config ConfigureS3Client()
    {
        var s3Config = new AmazonS3Config();
        s3Config.ServiceURL = _options.S3ServiceUrl;
        
        // Needed for integration testing with S3Mock, see https://github.com/adobe/S3Mock/blob/main/README.md#s3mock
        s3Config.ForcePathStyle = true;

        return s3Config;
    }
    
    private AmazonS3Client ConfigureS3Client(AmazonS3Config s3Config)
    {
        return new AmazonS3Client(
            _options.S3AccessKey, 
            _options.S3SecretKey,
            s3Config);
    }
}