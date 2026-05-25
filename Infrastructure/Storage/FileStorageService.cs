using ApiCargaArchivos.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace ApiCargaArchivos.Infrastructure.Storage;

public class FileStorageService : IFileStorageService
{
    private readonly string _uploadFolder;

    public FileStorageService(IConfiguration configuration, IWebHostEnvironment env)
    {
        var uploadPathConfig = configuration.GetValue<string>("FileStorage:UploadPath") ?? "Uploads";
        _uploadFolder = Path.IsPathRooted(uploadPathConfig) 
            ? uploadPathConfig 
            : Path.Combine(env.ContentRootPath, uploadPathConfig);
            
        if (!Directory.Exists(_uploadFolder)) 
            Directory.CreateDirectory(_uploadFolder);
    }

    public async Task<string> SaveFileAsync(Stream content, string fileName, CancellationToken cancellationToken)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var filePath = Path.Combine(_uploadFolder, uniqueFileName);

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
        await content.CopyToAsync(fileStream, cancellationToken);

        return filePath;
    }

    public Task<Stream> GetFileStreamAsync(string filePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("El archivo físico no existe.", filePath);

        Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
