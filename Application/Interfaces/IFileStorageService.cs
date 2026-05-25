namespace ApiCargaArchivos.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream content, string fileName, CancellationToken cancellationToken);
    Task<Stream> GetFileStreamAsync(string filePath, CancellationToken cancellationToken);
    void DeleteFile(string filePath);
}
