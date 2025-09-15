# TodoApp API

N-Layer mimarisi ile geliştirilmiş bir TODO API projesi. MSSQL veritabanı kullanır ve Entity Framework Core ile çalışır.

## Proje Yapısı

```
TodoApp/
├── src/
│   ├── TodoApp.Domain/          # Domain katmanı
│   │   ├── Entities/            # Domain entities
│   │   └── Interfaces/          # Repository interfaces
│   ├── TodoApp.Infrastructure/  # Infrastructure katmanı
│   │   ├── Data/               # DbContext
│   │   └── Repositories/       # Repository implementations
│   ├── TodoApp.Application/     # Application katmanı
│   │   ├── DTOs/               # Data Transfer Objects
│   │   ├── Interfaces/         # Service interfaces
│   │   ├── Services/           # Business logic
│   │   ├── Mappings/           # AutoMapper profiles
│   │   └── Validators/         # FluentValidation validators
│   └── TodoApp.API/            # API katmanı
│       └── Controllers/        # API Controllers
└── TodoApp.sln                 # Solution file
```

## Gereksinimler

- .NET 8.0 SDK
- SQL Server LocalDB (veya SQL Server)
- Visual Studio 2022 veya VS Code

## Kurulum

1. Projeyi klonlayın veya indirin
2. Terminal/Command Prompt'ta proje dizinine gidin
3. Veritabanı migration'larını oluşturun:

```bash
cd src/TodoApp.API
dotnet ef migrations add InitialCreate --project ../TodoApp.Infrastructure --startup-project .
```

4. Veritabanını oluşturun:

```bash
dotnet ef database update
```

5. Projeyi çalıştırın:

```bash
dotnet run
```

API, `https://localhost:7000` adresinde çalışacaktır.

## API Endpoints

### Todos

- `GET /api/todos` - Tüm todo'ları getir
- `GET /api/todos/{id}` - ID'ye göre todo getir
- `GET /api/todos/completed` - Tamamlanan todo'ları getir
- `GET /api/todos/pending` - Bekleyen todo'ları getir
- `GET /api/todos/priority/{priority}` - Önceliğe göre todo'ları getir
- `GET /api/todos/overdue` - Süresi geçmiş todo'ları getir
- `POST /api/todos` - Yeni todo oluştur
- `PUT /api/todos/{id}` - Todo'yu güncelle
- `PATCH /api/todos/{id}/complete` - Todo'yu tamamlandı olarak işaretle
- `PATCH /api/todos/{id}/pending` - Todo'yu bekleyen olarak işaretle
- `DELETE /api/todos/{id}` - Todo'yu sil

## Swagger UI

Proje çalıştığında Swagger UI'ya `https://localhost:7000` adresinden erişebilirsiniz.

## Veritabanı

Proje SQL Server LocalDB kullanır. Connection string `appsettings.json` dosyasında tanımlanmıştır:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoAppDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Özellikler

- N-Layer Architecture
- Entity Framework Core
- AutoMapper
- FluentValidation
- Swagger/OpenAPI
- CORS desteği
- Logging
- Seed data
