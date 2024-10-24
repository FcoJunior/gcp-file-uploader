using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using MentionUploader.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MentionUploader.Infra.Storage;

public sealed class GcpFileUploader : IFileUploader
{
    private readonly string _bucketName;
    private readonly string _pathName;
    private readonly StorageClient _storageClient;

    public GcpFileUploader(IConfiguration configuration)
    {
        _bucketName = configuration["GoogleCloudStorage:BucketName"];
        _pathName = configuration["GoogleCloudStorage:PathName"];
        var credentials = GoogleCredential.FromFile(configuration["GoogleCloudStorage:CredentialsFilePath"]);
        _storageClient = StorageClient.Create(credentials);
    }

    public async Task UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        var fileWithPath = $"{_pathName}/{fileName}";
        await _storageClient.UploadObjectAsync(_bucketName, fileWithPath, null, fileStream, cancellationToken: cancellationToken);
    }
    
    public async Task<List<string>> ListFilesAsync(CancellationToken cancellationToken)
    {
        List<string> fileNames = [];

        try
        {
            var objects = _storageClient.ListObjectsAsync(_bucketName, _pathName);

            await foreach (var obj in objects)
            {
                fileNames.Add(obj.Name);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao listar arquivos: {ex.Message}");
        }

        return fileNames;
    }
}