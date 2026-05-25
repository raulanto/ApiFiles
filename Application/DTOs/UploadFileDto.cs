namespace ApiCargaArchivos.Application.DTOs;

public record UploadFileDto(
    Stream Content,
    string FileName,
    string ContentType,
    long Length
);
