# TodoApp API - N-Layer Architecture

N-Layer mimarisi ile geliştirilmiş bir TODO API projesi. MSSQL veritabanı kullanır ve Entity Framework Core ile çalışır.

## 🎯 Proje Amacı

Bu proje, stajyerlere **N-Layer Architecture** (Katmanlı Mimari) kavramlarını öğretmek için tasarlanmıştır. Gerçek dünya projelerinde kullanılan modern .NET teknolojilerini içerir.

## 📚 N-Layer Architecture Nedir?

N-Layer Architecture, uygulamayı mantıksal katmanlara ayıran bir mimari desendir. Her katmanın kendine özgü sorumluluğu vardır:

- **Separation of Concerns**: Her katman kendi işinden sorumludur
- **Maintainability**: Kod bakımı kolaylaşır
- **Testability**: Her katman ayrı ayrı test edilebilir
- **Scalability**: Yeni özellikler kolayca eklenebilir

## 🏗️ Proje Yapısı

```
TodoApp/
├── src/
│   ├── TodoApp.Domain/          # 🎯 Domain Katmanı (İş Kuralları)
│   │   ├── Entities/            # 📊 Veri modelleri (Todo.cs)
│   │   └── Interfaces/          # 🔌 Repository interface'leri
│   ├── TodoApp.Infrastructure/  # 🗄️ Infrastructure Katmanı (Veri Erişimi)
│   │   ├── Data/               # 🗃️ DbContext (Veritabanı bağlantısı)
│   │   └── Repositories/       # 📦 Repository implementasyonları
│   ├── TodoApp.Application/     # ⚙️ Application Katmanı (İş Mantığı)
│   │   ├── DTOs/               # 📋 Data Transfer Objects
│   │   ├── Interfaces/         # 🔌 Service interface'leri
│   │   ├── Services/           # 🛠️ İş mantığı servisleri
│   │   ├── Mappings/           # 🔄 AutoMapper profilleri
│   │   └── Validators/         # ✅ FluentValidation kuralları
│   └── TodoApp.API/            # 🌐 API Katmanı (Sunum)
│       └── Controllers/        # 🎮 REST API Controller'ları
└── TodoApp.sln                 # 📁 Solution dosyası
```

## 🔄 Katmanlar Arası İletişim

```
API Layer (Controllers)
    ↓ (HTTP Requests)
Application Layer (Services)
    ↓ (Business Logic)
Infrastructure Layer (Repositories)
    ↓ (Data Access)
Domain Layer (Entities)
    ↓ (Database)
SQL Server Database
```

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Açıklama | Katman |
|-----------|----------|---------|
| **.NET 8.0** | Framework | Tüm katmanlar |
| **Entity Framework Core** | ORM | Infrastructure |
| **SQL Server** | Veritabanı | Infrastructure |
| **AutoMapper** | Object Mapping | Application |
| **FluentValidation** | Validation | Application |
| **Swagger/OpenAPI** | API Dokümantasyonu | API |
| **Dependency Injection** | IoC Container | Tüm katmanlar |

## 📋 Gereksinimler

- **.NET 8.0 SDK** - [İndir](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server LocalDB** - Visual Studio ile birlikte gelir
- **Visual Studio 2022** veya **VS Code** - IDE
- **Git** - Versiyon kontrolü

## 🚀 Kurulum Adımları

### 1. Projeyi İndirin
```bash
git clone https://github.com/Dca3/TODOL-ST.git
cd TODOL-ST
```

### 2. Veritabanı Migration'larını Oluşturun
```bash
cd src/TodoApp.API
dotnet ef migrations add InitialCreate --project ../TodoApp.Infrastructure --startup-project .
```

### 3. Veritabanını Oluşturun
```bash
dotnet ef database update
```

### 4. Projeyi Çalıştırın
```bash
dotnet run
```

### 5. API'ye Erişin
- **Swagger UI**: `https://localhost:7000`
- **API Base URL**: `https://localhost:7000/api`

## 🎓 Stajyerler İçin Öğrenme Rehberi

### 1. Domain Katmanı (İş Kuralları)
- **Entities**: Veri modellerini inceleyin (`Todo.cs`)
- **Interfaces**: Repository pattern'i anlayın
- **Sorumluluk**: İş kuralları ve veri yapısı

### 2. Infrastructure Katmanı (Veri Erişimi)
- **DbContext**: Entity Framework konfigürasyonu
- **Repositories**: Veri erişim implementasyonları
- **Sorumluluk**: Veritabanı işlemleri

### 3. Application Katmanı (İş Mantığı)
- **Services**: İş mantığı implementasyonları
- **DTOs**: Veri transfer objeleri
- **Mappings**: AutoMapper konfigürasyonu
- **Validators**: FluentValidation kuralları
- **Sorumluluk**: İş mantığı ve validasyon

### 4. API Katmanı (Sunum)
- **Controllers**: REST API endpoint'leri
- **Program.cs**: Uygulama konfigürasyonu
- **Sorumluluk**: HTTP isteklerini karşılama

## 🔍 Kod İnceleme Önerileri

1. **Dependency Injection** nasıl çalışıyor?
2. **Repository Pattern** neden kullanılıyor?
3. **DTO** ve **Entity** arasındaki fark nedir?
4. **AutoMapper** nasıl çalışıyor?
5. **FluentValidation** nasıl entegre edilmiş?
6. **Exception Handling** nasıl yapılmış?

## 🌐 API Endpoints

### 📋 Todos

| Method | Endpoint | Açıklama | HTTP Status |
|--------|----------|----------|-------------|
| `GET` | `/api/todos` | Tüm todo'ları getir | 200 OK |
| `GET` | `/api/todos/{id}` | ID'ye göre todo getir | 200 OK / 404 Not Found |
| `GET` | `/api/todos/completed` | Tamamlanan todo'ları getir | 200 OK |
| `GET` | `/api/todos/pending` | Bekleyen todo'ları getir | 200 OK |
| `GET` | `/api/todos/priority/{priority}` | Önceliğe göre todo'ları getir | 200 OK |
| `GET` | `/api/todos/overdue` | Süresi geçmiş todo'ları getir | 200 OK |
| `POST` | `/api/todos` | Yeni todo oluştur | 201 Created |
| `PUT` | `/api/todos/{id}` | Todo'yu güncelle | 200 OK / 404 Not Found |
| `PATCH` | `/api/todos/{id}/complete` | Todo'yu tamamlandı işaretle | 200 OK |
| `PATCH` | `/api/todos/{id}/pending` | Todo'yu bekleyen işaretle | 200 OK |
| `DELETE` | `/api/todos/{id}` | Todo'yu sil | 204 No Content |

### 📝 Örnek API Kullanımı

#### Yeni Todo Oluştur
```json
POST /api/todos
{
  "title": "Yeni Görev",
  "description": "Bu görevin açıklaması",
  "priority": 2,
  "dueDate": "2024-12-31T23:59:59Z"
}
```

#### Todo Güncelle
```json
PUT /api/todos/1
{
  "title": "Güncellenmiş Görev",
  "description": "Güncellenmiş açıklama",
  "isCompleted": true,
  "priority": 3,
  "dueDate": "2024-12-25T12:00:00Z"
}
```

## 📊 Swagger UI

Proje çalıştığında Swagger UI'ya `https://localhost:7000` adresinden erişebilirsiniz. Swagger, API'nin interaktif dokümantasyonunu sağlar.

## 🗄️ Veritabanı

Proje SQL Server LocalDB kullanır. Connection string `appsettings.json` dosyasında tanımlanmıştır:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoAppDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## ✨ Özellikler

- ✅ **N-Layer Architecture** - Katmanlı mimari
- ✅ **Entity Framework Core** - ORM
- ✅ **AutoMapper** - Object mapping
- ✅ **FluentValidation** - Giriş doğrulama
- ✅ **Swagger/OpenAPI** - API dokümantasyonu
- ✅ **CORS** - Cross-origin resource sharing
- ✅ **Logging** - Hata loglama
- ✅ **Seed Data** - Örnek veriler
- ✅ **Dependency Injection** - IoC container
- ✅ **Repository Pattern** - Veri erişim deseni

## 🎯 Stajyerler İçin Hedefler

Bu projeyi inceledikten sonra şunları öğrenmiş olacaksınız:

1. **N-Layer Architecture** kavramını
2. **Dependency Injection** kullanımını
3. **Repository Pattern** implementasyonunu
4. **Entity Framework Core** ile veritabanı işlemlerini
5. **AutoMapper** ile object mapping'i
6. **FluentValidation** ile giriş doğrulamayı
7. **RESTful API** tasarımını
8. **Swagger** ile API dokümantasyonunu

## 🤝 Katkıda Bulunma

Bu proje eğitim amaçlıdır. Geliştirme önerileri için issue açabilirsiniz.

## 📄 Lisans

Bu proje eğitim amaçlıdır ve açık kaynak kodludur.
