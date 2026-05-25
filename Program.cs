using ApiCargaArchivos.Application.Interfaces;
using ApiCargaArchivos.Application.UseCases;
using ApiCargaArchivos.Infrastructure.Repositories;
using ApiCargaArchivos.Infrastructure.Storage;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using System.Reflection;

// Configuración inicial de Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando aplicación");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    builder.Services.AddControllers();

    builder.Services.AddScoped<IFileRepository, FileRepository>();
    builder.Services.AddScoped<IFileStorageService, FileStorageService>();
    builder.Services.AddScoped<IUploadFileUseCase, UploadFileUseCase>();
    builder.Services.AddScoped<IGetFileUseCase, GetFileUseCase>();
    builder.Services.AddScoped<IDeleteFileUseCase, DeleteFileUseCase>();
    builder.Services.AddScoped<IUpdateFileUseCase, UpdateFileUseCase>();

    builder.Services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();
    builder.Services.AddValidatorsFromAssemblyContaining<IUploadFileUseCase>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Carga Archivos v1");
        c.RoutePrefix = "swagger";
    });

    app.UseCors("AllowAll");

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fallo al iniciar la aplicación");
}
finally
{
    Log.CloseAndFlush();
}
