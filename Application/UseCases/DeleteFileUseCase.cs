using ApiCargaArchivos.Application.Interfaces;

namespace ApiCargaArchivos.Application.UseCases;

public class DeleteFileUseCase(IFileRepository repository, IFileStorageService storageService) : IDeleteFileUseCase
{
    public async Task ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var fileRecord = await repository.GetByIdAsync(id, cancellationToken);
        if (fileRecord == null)
            throw new KeyNotFoundException("Registro de archivo no encontrado.");

        storageService.DeleteFile(fileRecord.StoredPath);
        await repository.DeleteAsync(id, cancellationToken);
    }
}
