using System.Data;
using Dapper;
using Npgsql;
using ApiCargaArchivos.Application.Interfaces;
using ApiCargaArchivos.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ApiCargaArchivos.Infrastructure.Repositories;

public class FileRepository(IConfiguration configuration) : IFileRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new ArgumentNullException("Connection string not found");

    public async Task<FileRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT id as Id, original_name as OriginalName, stored_path as StoredPath, size_in_bytes as SizeInBytes, uploaded_at as UploadedAt FROM uploaded_files WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<FileRecord>(sql, new { Id = id });
    }

    public async Task SaveAsync(FileRecord fileRecord, CancellationToken cancellationToken)
    {
        const string sql = @"
            INSERT INTO uploaded_files (id, original_name, stored_path, size_in_bytes, uploaded_at) 
            VALUES (@Id, @OriginalName, @StoredPath, @SizeInBytes, @UploadedAt)";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, fileRecord);
    }

    public async Task UpdateAsync(FileRecord fileRecord, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE uploaded_files 
            SET original_name = @OriginalName, 
                stored_path = @StoredPath, 
                size_in_bytes = @SizeInBytes, 
                uploaded_at = @UploadedAt 
            WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, fileRecord);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM uploaded_files WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
