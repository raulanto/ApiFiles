using ApiCargaArchivos.Application.Interfaces;
using ApiCargaArchivos.Domain.Entities;
using ApiCargaArchivos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCargaArchivos.Infrastructure.Repositories;

public class FileRepository(ApplicationDbContext context) : IFileRepository
{
    public async Task<FileRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.UploadedFiles
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task SaveAsync(FileRecord fileRecord, CancellationToken cancellationToken)
    {
        await context.UploadedFiles.AddAsync(fileRecord, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FileRecord fileRecord, CancellationToken cancellationToken)
    {
        context.UploadedFiles.Update(fileRecord);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var record = await context.UploadedFiles.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        if (record != null)
        {
            context.UploadedFiles.Remove(record);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
