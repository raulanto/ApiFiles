using ApiCargaArchivos.Application.DTOs;
using ApiCargaArchivos.Domain.Entities;

namespace ApiCargaArchivos.Application.Interfaces;

public interface IUploadFileUseCase
{
    Task<FileRecord> ExecuteAsync(UploadFileDto dto, CancellationToken cancellationToken);
}
