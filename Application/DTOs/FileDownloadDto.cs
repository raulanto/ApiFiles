namespace ApiCargaArchivos.Application.DTOs;

public record FileDownloadDto(Stream Content, string OriginalName);
