namespace MentionUploader.Application.Interfaces;

public interface IFileUploader
{
    Task UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
    Task<List<string>> ListFilesAsync(CancellationToken cancellationToken);
}