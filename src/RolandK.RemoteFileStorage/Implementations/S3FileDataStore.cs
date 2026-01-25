using Amazon.S3;
using Amazon.S3.Transfer;
using RolandK.RemoteFileStorage.Util;

namespace RolandK.RemoteFileStorage.Implementations;

internal class S3FileDataStore : IFileDataStore
{
    private readonly FileDataStoreOptions _options;
    
    public S3FileDataStore(FileDataStoreOptions options)
    {
        _options = options;
    }

    public async Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var s3Config = new AmazonS3Config();
        s3Config.ServiceURL = _options.ServiceUrl;
        
        // Needed for integration testing with S3Mock, see https://github.com/adobe/S3Mock/blob/main/README.md#s3mock
        s3Config.ForcePathStyle = true;
        
        var s3Client = new Amazon.S3.AmazonS3Client(
            _options.AccessKey, 
            _options.SecretKey,
            s3Config);

        TransferUtility? transferUtility = null;
        Stream? resultStream = null;
        try
        {
            transferUtility = new TransferUtility(s3Client);
            resultStream = await transferUtility.OpenStreamAsync(_options.BucketName, filePath, cancellationToken);

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
}