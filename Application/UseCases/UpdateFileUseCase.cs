using ApiCargaArchivos.Application.DTOs;
using ApiCargaArchivos.Application.Interfaces;
using ApiCargaArchivos.Domain.Entities;
using FluentValidation;

namespace ApiCargaArchivos.Application.UseCases;

public class UpdateFileUseCase(
    IFileStorageService storageService, 
    IFileRepository repository,
    IValidator<UpdateFileDto> validator) : IUpdateFileUseCase
{
    public async Task<FileRecord> ExecuteAsync(UpdateFileDto dto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(dto, cancellationToken);

        var fileRecord = await repository.GetByIdAsync(dto.Id, cancellationToken);
        if (fileRecord == null)
            throw new KeyNotFoundException("Registro de archivo no encontrado.");

        // Eliminar el físico anterior
        storageService.DeleteFile(fileRecord.StoredPath);

        // Guardar el nuevo físico
        var newStoredPath = await storageService.SaveFileAsync(dto.Content, dto.FileName, cancellationToken);

        // Actualizar registro
        fileRecord.Update(dto.FileName, newStoredPath, dto.Length);
        await repository.UpdateAsync(fileRecord, cancellationToken);

        return fileRecord;
    }
}
