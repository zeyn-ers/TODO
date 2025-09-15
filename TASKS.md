# 🎯 TodoApp - Stajyer Görevleri

Bu doküman, TodoApp projesi üzerinde çalışarak N-Layer Architecture kavramlarını öğrenmeniz için tasarlanmış görevleri içerir.

## 📚 Öğrenme Hedefleri

Bu görevleri tamamladıktan sonra şunları öğrenmiş olacaksınız:
- N-Layer Architecture kavramını
- Dependency Injection kullanımını
- Repository Pattern implementasyonunu
- Entity Framework Core ile veritabanı işlemlerini
- AutoMapper ile object mapping'i
- FluentValidation ile giriş doğrulamayı
- RESTful API tasarımını

---

## 🚀 Başlangıç Görevleri

### Task 1: Projeyi Çalıştırma ve İnceleme
**Süre:** 30 dakika  
**Zorluk:** ⭐

1. Projeyi local bilgisayarınıza klonlayın
2. Veritabanı migration'larını oluşturun ve çalıştırın
3. Projeyi çalıştırın ve Swagger UI'ya erişin
4. Mevcut API endpoint'lerini test edin
5. Veritabanında oluşan tabloları inceleyin

**Beklenen Sonuç:** Proje başarıyla çalışıyor ve API endpoint'leri test edilebiliyor.

---

## 🏗️ Domain Katmanı Görevleri

### Task 2: Yeni Entity Ekleme
**Süre:** 45 dakika  
**Zorluk:** ⭐⭐

**Görev:** `Category` entity'si ekleyin

1. `TodoApp.Domain/Entities/` klasörüne `Category.cs` dosyası oluşturun
2. Category entity'si için gerekli property'leri tanımlayın:
   - `Id` (int)
   - `Name` (string, max 100 karakter)
   - `Description` (string, max 500 karakter)
   - `CreatedAt` (DateTime)
   - `IsActive` (bool)

3. Todo entity'sine `CategoryId` ve `Category` navigation property'si ekleyin

**Beklenen Sonuç:** Category entity'si oluşturuldu ve Todo ile ilişkilendirildi.

### Task 3: Repository Interface Genişletme
**Süre:** 30 dakika  
**Zorluk:** ⭐⭐

**Görev:** Category için repository interface'i oluşturun

1. `TodoApp.Domain/Interfaces/` klasörüne `ICategoryRepository.cs` dosyası oluşturun
2. `IRepository<Category>` interface'inden türetin
3. Category'ye özel metodlar ekleyin:
   - `GetActiveCategoriesAsync()`
   - `GetCategoryByNameAsync(string name)`

**Beklenen Sonuç:** Category repository interface'i oluşturuldu.

---

## 🗄️ Infrastructure Katmanı Görevleri

### Task 4: DbContext Güncelleme
**Süre:** 45 dakika  
**Zorluk:** ⭐⭐

**Görev:** DbContext'e Category desteği ekleyin

1. `TodoDbContext.cs` dosyasını güncelleyin
2. `DbSet<Category> Categories` property'si ekleyin
3. `OnModelCreating` metodunda Category için konfigürasyon ekleyin
4. Todo ve Category arasındaki ilişkiyi tanımlayın
5. Category için seed data ekleyin

**Beklenen Sonuç:** DbContext Category entity'sini destekliyor.

### Task 5: Repository Implementation
**Süre:** 60 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** CategoryRepository implementasyonu

1. `TodoApp.Infrastructure/Repositories/` klasörüne `CategoryRepository.cs` dosyası oluşturun
2. `BaseRepository<Category>` sınıfından türetin
3. `ICategoryRepository` interface'ini implement edin
4. Özel metodları implement edin
5. DependencyInjection.cs dosyasına CategoryRepository'yi ekleyin

**Beklenen Sonuç:** CategoryRepository implementasyonu tamamlandı.

---

## ⚙️ Application Katmanı Görevleri

### Task 6: DTO'lar Oluşturma
**Süre:** 45 dakika  
**Zorluk:** ⭐⭐

**Görev:** Category için DTO'lar oluşturun

1. `TodoApp.Application/DTOs/` klasörüne Category DTO'larını ekleyin:
   - `CategoryDto`
   - `CreateCategoryDto`
   - `UpdateCategoryDto`
2. TodoDto'ya `CategoryId` ve `CategoryName` property'leri ekleyin
3. Tüm DTO'lara XML yorumları ekleyin

**Beklenen Sonuç:** Category DTO'ları oluşturuldu.

### Task 7: Service Interface ve Implementation
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** CategoryService oluşturun

1. `ITodoService` interface'ini güncelleyin:
   - `GetTodosByCategoryAsync(int categoryId)` metodu ekleyin
2. `TodoApp.Application/Interfaces/` klasörüne `ICategoryService.cs` oluşturun
3. `TodoApp.Application/Services/` klasörüne `CategoryService.cs` oluşturun
4. CategoryService'i implement edin
5. DependencyInjection.cs dosyasına CategoryService'i ekleyin

**Beklenen Sonuç:** CategoryService implementasyonu tamamlandı.

### Task 8: AutoMapper Profilleri
**Süre:** 30 dakika  
**Zorluk:** ⭐⭐

**Görev:** Category için mapping profilleri ekleyin

1. `MappingProfile.cs` dosyasını güncelleyin
2. Category entity ve DTO'ları arasında mapping kuralları ekleyin
3. Todo entity'sine Category mapping'i ekleyin

**Beklenen Sonuç:** Category mapping profilleri oluşturuldu.

### Task 9: Validation Kuralları
**Süre:** 45 dakika  
**Zorluk:** ⭐⭐

**Görev:** Category için validation kuralları ekleyin

1. `TodoApp.Application/Validators/` klasörüne Category validator'larını ekleyin:
   - `CreateCategoryDtoValidator`
   - `UpdateCategoryDtoValidator`
2. Validation kurallarını tanımlayın
3. XML yorumları ekleyin

**Beklenen Sonuç:** Category validation kuralları oluşturuldu.

---

## 🌐 API Katmanı Görevleri

### Task 10: Category Controller
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** CategoryController oluşturun

1. `TodoApp.API/Controllers/` klasörüne `CategoriesController.cs` oluşturun
2. CRUD operasyonları için endpoint'ler ekleyin:
   - `GET /api/categories` - Tüm kategoriler
   - `GET /api/categories/{id}` - ID'ye göre kategori
   - `GET /api/categories/active` - Aktif kategoriler
   - `POST /api/categories` - Yeni kategori oluştur
   - `PUT /api/categories/{id}` - Kategori güncelle
   - `DELETE /api/categories/{id}` - Kategori sil
3. Tüm metodlara XML yorumları ekleyin
4. Exception handling ekleyin

**Beklenen Sonuç:** CategoryController tamamlandı.

### Task 11: Todo Controller Güncelleme
**Süre:** 60 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** TodoController'a kategori desteği ekleyin

1. `TodosController.cs` dosyasını güncelleyin
2. Yeni endpoint ekleyin:
   - `GET /api/todos/category/{categoryId}` - Kategoriye göre todo'lar
3. Mevcut endpoint'leri güncelleyin (kategori bilgilerini dahil et)

**Beklenen Sonuç:** TodoController kategori desteği ile güncellendi.

---

## 🔧 Gelişmiş Görevler

### Task 12: Pagination Ekleme
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Todo listesi için pagination ekleyin

1. `PagedResult<T>` generic sınıfı oluşturun
2. Repository'lerde pagination desteği ekleyin
3. Service'lerde pagination metodları ekleyin
4. Controller'larda pagination parametrelerini kabul edin
5. Swagger dokümantasyonunu güncelleyin

**Beklenen Sonuç:** Pagination sistemi çalışıyor.

### Task 13: Logging Sistemi
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** Structured logging ekleyin

1. Serilog paketini ekleyin
2. Program.cs'de Serilog konfigürasyonu yapın
3. Controller'larda detaylı logging ekleyin
4. Service'lerde hata loglama ekleyin
5. Log dosyalarını inceleyin

**Beklenen Sonuç:** Logging sistemi çalışıyor.

### Task 14: Unit Test Yazma
**Süre:** 150 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Unit test'ler yazın

1. Test projesi oluşturun
2. Service'ler için unit test'ler yazın
3. Repository'ler için unit test'ler yazın
4. Mock objeler kullanın
5. Test coverage'ı inceleyin

**Beklenen Sonuç:** Unit test'ler yazıldı ve çalışıyor.

---

## 🎨 Bonus Görevler

### Task 15: API Versioning
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** API versioning ekleyin

1. API versioning paketini ekleyin
2. V1 ve V2 controller'ları oluşturun
3. Swagger'da version desteği ekleyin
4. Backward compatibility sağlayın

### Task 16: Caching Sistemi
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Memory cache ekleyin

1. IMemoryCache kullanın
2. Category listesi için cache ekleyin
3. Cache invalidation stratejisi uygulayın
4. Performance test'leri yapın

### Task 17: Rate Limiting
**Süre:** 60 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** API rate limiting ekleyin

1. Rate limiting middleware ekleyin
2. Endpoint'lere farklı limit'ler uygulayın
3. Rate limit bilgilerini response header'da döndürün

---



---



### Faydalı Kaynaklar:
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [AutoMapper Docs](https://docs.automapper.org/)
- [FluentValidation Docs](https://docs.fluentvalidation.net/)
- [ASP.NET Core Docs](https://docs.microsoft.com/en-us/aspnet/core/)

---

## 🎯 Başarı Kriterleri

Bu görevleri tamamladıktan sonra:
- ✅ N-Layer Architecture kavramını anlayacaksınız
- ✅ Modern .NET teknolojilerini kullanabileceksiniz
- ✅ Clean Code prensiplerini uygulayabileceksiniz
- ✅ API tasarımı yapabileceksiniz
- ✅ Veritabanı işlemlerini gerçekleştirebileceksiniz

**Başarılar! 🚀**
