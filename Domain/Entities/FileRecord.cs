namespace ApiCargaArchivos.Domain.Entities;

public class FileRecord
{
    public Guid Id { get; private set; }
    public string OriginalName { get; private set; }
    public string StoredPath { get; private set; }
    public long SizeInBytes { get; private set; }
    public DateTime UploadedAt { get; private set; }

    // Constructor requerido por Dapper/ORMs para materialización
    private FileRecord() 
    { 
        OriginalName = null!;
        StoredPath = null!;
    }

    public FileRecord(Guid id, string originalName, string storedPath, long sizeInBytes)
    {
        Id = id;
        OriginalName = originalName;
        StoredPath = storedPath;
        SizeInBytes = sizeInBytes;
        UploadedAt = DateTime.UtcNow;
    }

    public void Update(string originalName, string storedPath, long sizeInBytes)
    {
        OriginalName = originalName;
        StoredPath = storedPath;
        SizeInBytes = sizeInBytes;
        UploadedAt = DateTime.UtcNow;
    }
}
