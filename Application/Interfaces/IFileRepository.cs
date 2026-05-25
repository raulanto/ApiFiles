using ApiCargaArchivos.Domain.Entities;

namespace ApiCargaArchivos.Application.Interfaces;

public interface IFileRepository
{
    Task<FileRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task SaveAsync(FileRecord fileRecord, CancellationToken cancellationToken);
    Task UpdateAsync(FileRecord fileRecord, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
