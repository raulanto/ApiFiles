using ApiCargaArchivos.Application.DTOs;
using ApiCargaArchivos.Domain.Entities;

namespace ApiCargaArchivos.Application.Interfaces;

public interface IUpdateFileUseCase
{
    Task<FileRecord> ExecuteAsync(UpdateFileDto dto, CancellationToken cancellationToken);
}
