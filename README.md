# API Carga de Archivos (.NET 10)

API RESTful desarrollada en **.NET 10** bajo los principios de **Clean Architecture**, diseñada para gestionar el ciclo de vida completo de archivos (subida, consulta/descarga, actualización y eliminación) combinando almacenamiento físico en disco y persistencia de metadatos en **PostgreSQL** mediante **Entity Framework Core**.

---

## 🚀 ¿De qué trata el proyecto?

El proyecto resuelve la gestión eficiente y estructurada de archivos adjuntos o documentos en sistemas backend:
1. **Almacenamiento Físico**: Los archivos binarios se guardan en el sistema de archivos (disco local o volumen montado) asignándoles un nombre único (GUID) para prevenir colisiones o sobreescritura accidental.
2. **Persistencia de Metadatos**: Los metadatos asociados (identificador único, nombre original, ruta física, peso en bytes y fecha/hora UTC de carga) se guardan en una base de datos relacional PostgreSQL.
3. **Manejo Desacoplado**: Se implementa una separación estricta de responsabilidades a través de Casos de Uso (Use Cases), repositorios abstractos y servicios de almacenamiento independientes.

---

## 🏛️ Arquitectura del Sistema

El proyecto sigue un enfoque limpio y desacoplado:

```text
ApiFiles/
│
├── Domain/                  # Reglas de negocio y entidades puras
│   └── Entities/            # FileRecord (entidad con encapsulamiento)
│
├── Application/             # Casos de uso, interfaces y validaciones
│   ├── DTOs/                # Objetos de transferencia de datos
│   ├── Interfaces/          # Contratos de repositorios y servicios de storage
│   ├── UseCases/            # Lógica de aplicación (Upload, Get, Update, Delete)
│   └── Validators/          # Reglas con FluentValidation
│
├── Infrastructure/          # Acceso a datos y sistema de archivos
│   ├── Data/                # ApplicationDbContext (EF Core)
│   ├── Repositories/        # Implementación de persistencia con EF Core
│   └── Storage/             # Servicio de lectura/escritura en disco
│
└── Controllers/             # Endpoints HTTP (FilesController)
```

---

## 🛠️ Tecnologías y Librerías

- **Framework**: .NET 10 (ASP.NET Core Web API)
- **Base de Datos**: PostgreSQL
- **ORM**: [Entity Framework Core](https://learn.microsoft.com/ef/core/) (`Npgsql.EntityFrameworkCore.PostgreSQL`)
- **Validaciones**: [FluentValidation](https://fluentvalidation.net/)
- **Documentación de API**: Swagger / OpenAPI ([Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore))
- **Logging**: [Serilog](https://serilog.net/) (salida a consola y archivos rotativos en `/Logs`)

---

## 🗄️ Base de Datos y Docker

Puedes levantar PostgreSQL de forma automática usando **Docker Compose**. El contenedor ya incluye un script de inicialización ([`docker/init.sql`](file:///home/raulantodev/Projects/backend/ApiFiles/docker/init.sql)) que crea la tabla `uploaded_files` con las credenciales que coinciden con [appsettings.json](file:///home/raulantodev/Projects/backend/ApiFiles/appsettings.json).

### Iniciar la base de datos con Docker:
```bash
docker compose up -d
```

### Detener el contenedor:
```bash
docker compose down
```

### Esquema SQL utilizado:
```sql
CREATE TABLE IF NOT EXISTS uploaded_files (
    id UUID PRIMARY KEY,
    original_name VARCHAR(255) NOT NULL,
    stored_path TEXT NOT NULL,
    size_in_bytes BIGINT NOT NULL,
    uploaded_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT (NOW() AT TIME ZONE 'UTC')
);
```

---

## ⚙️ Configuración (`appsettings.json`)

Edita el archivo `appsettings.json` con tus credenciales y rutas de almacenamiento:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=carga_archivos;Username=tu_usuario;Password=tu_contraseña"
  },
  "FileStorage": {
    "UploadPath": "/ruta/a/tu/directorio/de/archivos"
  },
  "AllowedHosts": "*"
}
```

> **Nota**: `FileStorage:UploadPath` puede ser una ruta absoluta o una ruta relativa al directorio raíz de la aplicación. Si el directorio no existe, el servicio lo creará automáticamente al iniciar.

---

## 📡 Endpoints de la API

La API expone sus operaciones bajo la ruta base `/api/files`:

| Método | Endpoint | Consumes | Descripción |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/files/upload` | `multipart/form-data` | Sube un nuevo archivo al disco y registra sus metadatos. Devuelve el `fileId` generado. |
| `GET` | `/api/files/{id}` | N/A | Descarga el archivo binario utilizando el `id` asignado. |
| `PUT` | `/api/files/{id}` | `multipart/form-data` | Actualiza un archivo existente: reemplaza el archivo en disco y actualiza los metadatos en PostgreSQL. |
| `DELETE` | `/api/files/{id}` | N/A | Elimina el archivo físico de disco y borra el registro de la base de datos. |

### Ejemplos con `curl`

#### 1. Subir un archivo
```bash
curl -X POST "http://localhost:5056/api/files/upload" \
  -F "file=@/ruta/a/tu/documento.pdf"
```

**Respuesta (200 OK):**
```json
{
  "message": "Archivo subido exitosamente.",
  "fileId": "6f2d2a93-76d7-4632-9cbb-92e1fb5914fa",
  "originalName": "documento.pdf"
}
```

#### 2. Descargar un archivo por ID
```bash
curl -O -J -X GET "http://localhost:5056/api/files/6f2d2a93-76d7-4632-9cbb-92e1fb5914fa"
```

#### 3. Actualizar un archivo existente
```bash
curl -X PUT "http://localhost:5056/api/files/6f2d2a93-76d7-4632-9cbb-92e1fb5914fa" \
  -F "file=@/ruta/a/tu/nuevo_documento.pdf"
```

#### 4. Eliminar un archivo
```bash
curl -X DELETE "http://localhost:5056/api/files/6f2d2a93-76d7-4632-9cbb-92e1fb5914fa"
```

---

## 💻 Ejecución del Proyecto

1. **Restaurar dependencias**:
   ```bash
   dotnet restore
   ```

2. **Compilar el proyecto**:
   ```bash
   dotnet build
   ```

3. **Iniciar la aplicación**:
   ```bash
   dotnet run
   ```

4. **Acceder a la documentación Swagger**:
   Abre tu navegador en:
   ```text
   http://localhost:5056/swagger
   ```
