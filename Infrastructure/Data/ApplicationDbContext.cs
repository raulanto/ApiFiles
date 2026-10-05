using ApiCargaArchivos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiCargaArchivos.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<FileRecord> UploadedFiles => Set<FileRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileRecord>(entity =>
        {
            entity.ToTable("uploaded_files");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.OriginalName)
                .HasColumnName("original_name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.StoredPath)
                .HasColumnName("stored_path")
                .IsRequired();

            entity.Property(e => e.SizeInBytes)
                .HasColumnName("size_in_bytes")
                .IsRequired();

            entity.Property(e => e.UploadedAt)
                .HasColumnName("uploaded_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired();
        });
    }
}
