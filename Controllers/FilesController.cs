using ApiCargaArchivos.Application.DTOs;
using ApiCargaArchivos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCargaArchivos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController(
    IUploadFileUseCase uploadFileUseCase,
    IGetFileUseCase getFileUseCase,
    IDeleteFileUseCase deleteFileUseCase,
    IUpdateFileUseCase updateFileUseCase) : ControllerBase
{
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se proporcionó ningún archivo.");

        using var stream = file.OpenReadStream();
        
        var dto = new UploadFileDto(
            Content: stream,
            FileName: file.FileName,
            ContentType: file.ContentType,
            Length: file.Length
        );

        var result = await uploadFileUseCase.ExecuteAsync(dto, cancellationToken);

        return Ok(new { 
            message = "Archivo subido exitosamente.", 
            fileId = result.Id,
            originalName = result.OriginalName
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFile(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await getFileUseCase.ExecuteAsync(id, cancellationToken);
            return File(result.Content, "application/octet-stream", result.OriginalName);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await deleteFileUseCase.ExecuteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateFile(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No se proporcionó ningún archivo.");

        try
        {
            using var stream = file.OpenReadStream();
            
            var dto = new UpdateFileDto(
                Id: id,
                Content: stream,
                FileName: file.FileName,
                ContentType: file.ContentType,
                Length: file.Length
            );

            var result = await updateFileUseCase.ExecuteAsync(dto, cancellationToken);

            return Ok(new { 
                message = "Archivo actualizado exitosamente.", 
                fileId = result.Id,
                originalName = result.OriginalName
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
