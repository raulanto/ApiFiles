using ApiCargaArchivos.Application.DTOs;
using FluentValidation;

namespace ApiCargaArchivos.Application.Validators;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().WithMessage("El nombre del archivo es requerido.");
        RuleFor(x => x.Length).GreaterThan(0).WithMessage("El archivo no puede estar vacío.");
        RuleFor(x => x.Content).NotNull().WithMessage("El contenido del archivo no puede ser nulo.");
    }
}
