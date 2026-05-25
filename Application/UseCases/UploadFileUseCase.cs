using ApiCargaArchivos.Application.DTOs;
using ApiCargaArchivos.Application.Interfaces;
using ApiCargaArchivos.Domain.Entities;
using FluentValidation;

namespace ApiCargaArchivos.Application.UseCases;

public class UploadFileUseCase(
    IFileStorageService storageService, 
    IFileRepository repository,
    IValidator<UploadFileDto> validator) : IUploadFileUseCase
{
    public async Task<FileRecord> ExecuteAsync(UploadFileDto dto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(dto, cancellationToken);

        var storedPath = await storageService.SaveFileAsync(dto.Content, dto.FileName, cancellationToken);

        var fileRecord = new FileRecord(
            id: Guid.NewGuid(),
            originalName: dto.FileName,
            storedPath: storedPath,
            sizeInBytes: dto.Length
        );

        await repository.SaveAsync(fileRecord, cancellationToken);

        return fileRecord;
    }
}
