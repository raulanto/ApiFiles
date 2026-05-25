namespace ApiCargaArchivos.Application.DTOs;

public record UpdateFileDto(
    Guid Id,
    Stream Content,
    string FileName,
    string ContentType,
    long Length
);
