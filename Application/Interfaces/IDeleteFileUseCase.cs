namespace ApiCargaArchivos.Application.Interfaces;

public interface IDeleteFileUseCase
{
    Task ExecuteAsync(Guid id, CancellationToken cancellationToken);
}
