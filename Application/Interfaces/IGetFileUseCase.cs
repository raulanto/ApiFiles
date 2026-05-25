using ApiCargaArchivos.Application.DTOs;

namespace ApiCargaArchivos.Application.Interfaces;

public interface IGetFileUseCase
{
    Task<FileDownloadDto> ExecuteAsync(Guid id, CancellationToken cancellationToken);
}
