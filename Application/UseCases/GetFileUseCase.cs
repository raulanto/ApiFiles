using ApiCargaArchivos.Application.DTOs;
using ApiCargaArchivos.Application.Interfaces;

namespace ApiCargaArchivos.Application.UseCases;

public class GetFileUseCase(IFileRepository repository, IFileStorageService storageService) : IGetFileUseCase
{
    public async Task<FileDownloadDto> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var fileRecord = await repository.GetByIdAsync(id, cancellationToken);
        if (fileRecord == null)
            throw new KeyNotFoundException("Registro de archivo no encontrado.");

        var stream = await storageService.GetFileStreamAsync(fileRecord.StoredPath, cancellationToken);
        return new FileDownloadDto(stream, fileRecord.OriginalName);
    }
}
