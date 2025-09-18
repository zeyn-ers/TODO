# 🚀 TodoApp - Gelişmiş Stajyer Görevleri

Bu doküman, TodoApp projesi üzerinde çalışarak **gelişmiş** N-Layer Architecture kavramlarını ve **karmaşık** ilişkileri öğrenmeniz için tasarlanmış görevleri içerir.

> **Not:** Bu görevler, temel TASKS.md dosyasındaki görevleri tamamladıktan sonra yapılmalıdır.

## 📚 Öğrenme Hedefleri

Bu görevleri tamamladıktan sonra şunları öğrenmiş olacaksınız:
- Karmaşık veritabanı ilişkilerini (Many-to-Many, Self-Referencing)
- Advanced Entity Framework Core teknikleri
- Complex Query Operations (LINQ, Raw SQL)
- Performance Optimization teknikleri
- Advanced AutoMapper konfigürasyonları
- Custom Validation Rules
- Advanced API Design Patterns
- Microservices Architecture kavramları
- Caching ve Performance teknikleri
- Advanced Testing stratejileri

---

## 🏗️ Gelişmiş Domain Katmanı Görevleri

### Task 1: User Management System
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Kapsamlı kullanıcı yönetim sistemi oluşturun

#### 🎯 Kod Örneği - User Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/User.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// User entity - Kullanıcı bilgilerini tutan ana entity
/// </summary>
public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    [MaxLength(500)]
    public string? AvatarUrl { get; set; }
    
    [MaxLength(1000)]
    public string? Bio { get; set; }
    
    [MaxLength(100)]
    public string? Location { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;
    public UserRole Role { get; set; } = UserRole.User;
    
    // Navigation Properties
    public virtual UserProfile? Profile { get; set; }
    public virtual UserSettings? Settings { get; set; }
    public virtual ICollection<UserActivity> Activities { get; set; } = new List<UserActivity>();
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}

/// <summary>
/// User role enum - Kullanıcı rollerini tanımlar
/// </summary>
public enum UserRole
{
    SuperAdmin = 1,
    Admin = 2,
    Manager = 3,
    User = 4,
    Guest = 5
}
```

#### 🎯 Kod Örneği - UserProfile Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/UserProfile.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// UserProfile entity - Kullanıcı profil bilgilerini tutar
/// </summary>
public class UserProfile
{
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    [MaxLength(20)]
    public string? Gender { get; set; }
    
    [MaxLength(1000)]
    public string? Interests { get; set; } // JSON format
    
    [MaxLength(1000)]
    public string? Skills { get; set; } // JSON format
    
    [MaxLength(2000)]
    public string? SocialMediaLinks { get; set; } // JSON format
    
    [MaxLength(2000)]
    public string? Preferences { get; set; } // JSON format
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public virtual User User { get; set; } = null!;
}
```

#### 📝 Stajyer Görevi - UserSettings Entity

**Sizin yapmanız gerekenler:**

1. **UserSettings Entity** oluşturun:
   - `UserId`, `Theme`, `Language`, `Notifications`, `Privacy`
   - Navigation property'leri ekleyin
   - XML yorumları ekleyin

2. **UserActivity Entity** oluşturun:
   - `UserId`, `Action`, `Description`, `IpAddress`, `UserAgent`, `Timestamp`
   - Navigation property'leri ekleyin
   - XML yorumları ekleyin

3. **DbContext** güncelleyin:
   - Yeni entity'leri DbSet olarak ekleyin
   - OnModelCreating'de konfigürasyonları yapın

**Beklenen Sonuç:** Kapsamlı kullanıcı yönetim sistemi oluşturuldu.

### Task 2: Advanced Category System
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** Hiyerarşik kategori sistemi oluşturun

#### 🎯 Kod Örneği - Category Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/Category.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Category entity - Hiyerarşik kategori sistemi
/// </summary>
public class Category
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    // Self-referencing foreign key for hierarchy
    public int? ParentCategoryId { get; set; }
    
    public int Level { get; set; } = 0;
    
    [MaxLength(500)]
    public string Path { get; set; } = string.Empty; // e.g., "1/2/3"
    
    public int SortOrder { get; set; } = 0;
    
    [MaxLength(50)]
    public string? Icon { get; set; }
    
    [MaxLength(7)]
    public string? Color { get; set; } // Hex color
    
    public bool IsSystemCategory { get; set; } = false;
    public int UsageCount { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
    public virtual ICollection<CategoryPermission> Permissions { get; set; } = new List<CategoryPermission>();
    public virtual ICollection<CategoryTag> CategoryTags { get; set; } = new List<CategoryTag>();
}
```

#### 🎯 Kod Örneği - CategoryPermission Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/CategoryPermission.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// CategoryPermission entity - Kategori izin yönetimi
/// </summary>
public class CategoryPermission
{
    public int Id { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public PermissionType PermissionType { get; set; }
    
    [Required]
    public int GrantedBy { get; set; }
    
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    
    // Navigation Properties
    public virtual Category Category { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual User GrantedByUser { get; set; } = null!;
}

/// <summary>
/// Permission type enum - İzin türlerini tanımlar
/// </summary>
public enum PermissionType
{
    Read = 1,
    Write = 2,
    Delete = 3,
    Admin = 4
}
```

#### 📝 Stajyer Görevi - CategoryTag Entity

**Sizin yapmanız gerekenler:**

1. **CategoryTag Entity** oluşturun:
   - `CategoryId`, `TagId` (many-to-many junction table)
   - Navigation property'leri ekleyin
   - XML yorumları ekleyin

2. **DbContext** güncelleyin:
   - Category entity konfigürasyonunu ekleyin
   - Self-referencing ilişkiyi konfigüre edin
   - CategoryPermission ve CategoryTag konfigürasyonlarını ekleyin

3. **Seed Data** ekleyin:
   - Hiyerarşik kategori yapısı oluşturun
   - Örnek izinler ekleyin

**Beklenen Sonuç:** Hiyerarşik ve izin tabanlı kategori sistemi oluşturuldu.

### Task 3: Advanced Tag System
**Süre:** 75 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** Gelişmiş etiket sistemi oluşturun

#### 🎯 Kod Örneği - Tag Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/Tag.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Tag entity - Gelişmiş etiket sistemi
/// </summary>
public class Tag
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    // Self-referencing for tag hierarchy
    public int? ParentTagId { get; set; }
    
    [MaxLength(1000)]
    public string? Aliases { get; set; } // JSON array of aliases
    
    [MaxLength(1000)]
    public string? Synonyms { get; set; } // JSON array of synonyms
    
    public int UsageCount { get; set; } = 0;
    public decimal PopularityScore { get; set; } = 0;
    
    public bool IsSystemTag { get; set; } = false;
    
    [MaxLength(7)]
    public string? Color { get; set; } // Hex color
    
    [MaxLength(50)]
    public string? Icon { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public virtual Tag? ParentTag { get; set; }
    public virtual ICollection<Tag> SubTags { get; set; } = new List<Tag>();
    public virtual ICollection<TagRelationship> ParentRelationships { get; set; } = new List<TagRelationship>();
    public virtual ICollection<TagRelationship> ChildRelationships { get; set; } = new List<TagRelationship>();
    public virtual ICollection<TagUsage> Usages { get; set; } = new List<TagUsage>();
    public virtual ICollection<TodoTag> TodoTags { get; set; } = new List<TodoTag>();
    public virtual ICollection<CategoryTag> CategoryTags { get; set; } = new List<CategoryTag>();
}
```

#### 🎯 Kod Örneği - TagRelationship Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/TagRelationship.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// TagRelationship entity - Etiket ilişkilerini yönetir
/// </summary>
public class TagRelationship
{
    public int Id { get; set; }
    
    [Required]
    public int ParentTagId { get; set; }
    
    [Required]
    public int ChildTagId { get; set; }
    
    [Required]
    public TagRelationshipType RelationshipType { get; set; }
    
    public decimal Strength { get; set; } = 1.0m; // Relationship strength (0-1)
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public virtual Tag ParentTag { get; set; } = null!;
    public virtual Tag ChildTag { get; set; } = null!;
}

/// <summary>
/// Tag relationship type enum - Etiket ilişki türlerini tanımlar
/// </summary>
public enum TagRelationshipType
{
    Synonym = 1,    // Eş anlamlı
    Related = 2,    // İlgili
    SubTag = 3,     // Alt etiket
    ParentTag = 4,  // Üst etiket
    Alternative = 5 // Alternatif
}
```

#### 📝 Stajyer Görevi - TagUsage Entity

**Sizin yapmanız gerekenler:**

1. **TagUsage Entity** oluşturun:
   - `TagId`, `EntityType`, `EntityId`, `UsedBy`, `UsedAt`
   - Navigation property'leri ekleyin
   - XML yorumları ekleyin

2. **DbContext** güncelleyin:
   - Tag entity konfigürasyonunu ekleyin
   - Self-referencing ilişkiyi konfigüre edin
   - TagRelationship ve TagUsage konfigürasyonlarını ekleyin

3. **Repository Interface** oluşturun:
   - `ITagRepository` interface'ini genişletin
   - Gelişmiş sorgu metodları ekleyin

**Beklenen Sonuç:** Gelişmiş etiket sistemi ve ilişkileri oluşturuldu.

### Task 4: Comment System with Threading
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Threaded yorum sistemi oluşturun

#### 🎯 Kod Örneği - Comment Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/Comment.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Comment entity - Threaded yorum sistemi
/// </summary>
public class Comment
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public int TodoId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    // Self-referencing for threading
    public int? ParentCommentId { get; set; }
    
    public int ThreadId { get; set; } // Root comment ID for grouping
    public int Level { get; set; } = 0; // Thread level (0 = root, 1 = reply, etc.)
    
    [MaxLength(500)]
    public string Path { get; set; } = string.Empty; // e.g., "1/2/3" for threading
    
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    
    public int LikeCount { get; set; } = 0;
    public int DislikeCount { get; set; } = 0;
    public int ReportCount { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation Properties
    public virtual Todo Todo { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    public virtual ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();
    public virtual ICollection<CommentReport> Reports { get; set; } = new List<CommentReport>();
}
```

#### 🎯 Kod Örneği - CommentReaction Entity (Tamamlanmış)

```csharp
// TodoApp.Domain/Entities/CommentReaction.cs
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// CommentReaction entity - Yorum reaksiyonları
/// </summary>
public class CommentReaction
{
    public int Id { get; set; }
    
    [Required]
    public int CommentId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public ReactionType ReactionType { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation Properties
    public virtual Comment Comment { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

/// <summary>
/// Reaction type enum - Reaksiyon türlerini tanımlar
/// </summary>
public enum ReactionType
{
    Like = 1,
    Dislike = 2,
    Love = 3,
    Angry = 4,
    Laugh = 5,
    Sad = 6
}
```

#### 📝 Stajyer Görevi - CommentReport Entity

**Sizin yapmanız gerekenler:**

1. **CommentReport Entity** oluşturun:
   - `CommentId`, `ReportedBy`, `Reason`, `Description`, `Status`
   - `ReportedAt`, `ResolvedAt`, `ResolvedBy`
   - Navigation property'leri ekleyin
   - XML yorumları ekleyin

2. **DbContext** güncelleyin:
   - Comment entity konfigürasyonunu ekleyin
   - Self-referencing ilişkiyi konfigüre edin
   - CommentReaction ve CommentReport konfigürasyonlarını ekleyin

3. **Repository Interface** oluşturun:
   - `ICommentRepository` interface'ini genişletin
   - Threaded yorum sorguları için metodlar ekleyin

4. **Service Layer** oluşturun:
   - `ICommentService` interface'ini oluşturun
   - Threaded yorum işlemleri için metodlar ekleyin

**Beklenen Sonuç:** Threaded yorum sistemi ve reaksiyon sistemi oluşturuldu.

### Task 5: Advanced Todo System
**Süre:** 150 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Gelişmiş todo sistemi oluşturun

1. **Todo Entity** güncelleyin:
   - `ParentTodoId` (sub-tasks için)
   - `TemplateId`, `RecurrencePattern`, `EstimatedHours`
   - `ActualHours`, `ProgressPercentage`, `Status` (enum)
   - `Priority` (enum: Critical, High, Medium, Low)
   - `Complexity` (enum: Simple, Medium, Complex, VeryComplex)

2. **TodoTemplate Entity** oluşturun:
   - `Name`, `Description`, `DefaultCategoryId`, `DefaultTags`
   - `EstimatedHours`, `Steps`, `IsPublic`, `CreatedBy`

3. **TodoRecurrence Entity** oluşturun:
   - `TodoId`, `Pattern` (Daily, Weekly, Monthly, Yearly)
   - `Interval`, `DaysOfWeek`, `DayOfMonth`, `EndDate`

4. **TodoCollaborator Entity** oluşturun:
   - `TodoId`, `UserId`, `Role` (Owner, Assignee, Collaborator, Observer)
   - `AssignedAt`, `AssignedBy`

5. **TodoTimeLog Entity** oluşturun:
   - `TodoId`, `UserId`, `StartTime`, `EndTime`, `Duration`
   - `Description`, `IsBillable`

**Beklenen Sonuç:** Kapsamlı todo yönetim sistemi oluşturuldu.

---

## 🗄️ Gelişmiş Infrastructure Katmanı Görevleri

### Task 6: Advanced DbContext Configuration
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Gelişmiş DbContext konfigürasyonu

1. **Audit Trail** sistemi ekleyin:
   - `IAuditable` interface oluşturun
   - `AuditEntry` entity oluşturun
   - `SaveChanges` override edin

2. **Soft Delete** sistemi ekleyin:
   - `ISoftDeletable` interface oluşturun
   - Global query filter ekleyin

3. **Multi-tenancy** desteği ekleyin:
   - `ITenant` interface oluşturun
   - Tenant-based filtering ekleyin

4. **Concurrency Control** ekleyin:
   - `RowVersion` property'leri ekleyin
   - Optimistic concurrency handling

**Beklenen Sonuç:** Gelişmiş DbContext konfigürasyonu tamamlandı.

### Task 7: Advanced Repository Pattern
**Süre:** 150 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Gelişmiş repository pattern implementasyonu

#### 🎯 Kod Örneği - Specification Pattern (Tamamlanmış)

```csharp
// TodoApp.Domain/Interfaces/ISpecification.cs
using System.Linq.Expressions;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Specification pattern interface - Gelişmiş sorgu yapıları için
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Where koşulu
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }
    
    /// <summary>
    /// Include edilecek navigation property'ler
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }
    
    /// <summary>
    /// ThenInclude edilecek navigation property'ler
    /// </summary>
    List<string> IncludeStrings { get; }
    
    /// <summary>
    /// OrderBy koşulu
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }
    
    /// <summary>
    /// OrderByDescending koşulu
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }
    
    /// <summary>
    /// ThenBy koşulu
    /// </summary>
    Expression<Func<T, object>>? ThenBy { get; }
    
    /// <summary>
    /// ThenByDescending koşulu
    /// </summary>
    Expression<Func<T, object>>? ThenByDescending { get; }
    
    /// <summary>
    /// Take (limit) değeri
    /// </summary>
    int Take { get; }
    
    /// <summary>
    /// Skip (offset) değeri
    /// </summary>
    int Skip { get; }
    
    /// <summary>
    /// Pagination aktif mi
    /// </summary>
    bool IsPagingEnabled { get; }
}
```

#### 🎯 Kod Örneği - BaseSpecification (Tamamlanmış)

```csharp
// TodoApp.Domain/Interfaces/BaseSpecification.cs
using System.Linq.Expressions;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Base specification class - Specification pattern implementasyonu
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public Expression<Func<T, object>>? ThenBy { get; private set; }
    public Expression<Func<T, object>>? ThenByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected BaseSpecification()
    {
    }

    protected BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Include ekle
    /// </summary>
    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Include string ekle
    /// </summary>
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    /// <summary>
    /// OrderBy ekle
    /// </summary>
    protected virtual void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// OrderByDescending ekle
    /// </summary>
    protected virtual void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    /// <summary>
    /// Pagination ayarla
    /// </summary>
    protected virtual void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
```

#### 📝 Stajyer Görevi - Unit of Work Pattern

**Sizin yapmanız gerekenler:**

1. **IUnitOfWork Interface** oluşturun:
   - `IRepository<T>` metodları
   - `SaveChangesAsync()` metodu
   - `BeginTransactionAsync()` metodu
   - `CommitTransactionAsync()` metodu
   - `RollbackTransactionAsync()` metodu

2. **UnitOfWork Implementation** oluşturun:
   - `UnitOfWork` class'ını implement edin
   - Transaction management ekleyin
   - Repository'leri yönetin

3. **SpecificationEvaluator** oluşturun:
   - `SpecificationEvaluator<T>` class'ını oluşturun
   - Specification'ları IQueryable'a dönüştürün

4. **DependencyInjection** güncelleyin:
   - UnitOfWork'ü DI container'a ekleyin
   - Advanced repository'leri ekleyin

**Beklenen Sonuç:** Gelişmiş repository pattern implementasyonu tamamlandı.

### Task 8: Performance Optimization
**Süre:** 180 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Performance optimization teknikleri

1. **Database Indexing** stratejisi:
   - Composite index'ler oluşturun
   - Covering index'ler ekleyin
   - Index usage analizi

2. **Query Optimization**:
   - N+1 problem çözümü
   - Lazy loading vs Eager loading
   - Raw SQL query'ler

3. **Connection Pooling**:
   - Connection string optimization
   - Connection lifetime management

4. **Bulk Operations**:
   - Bulk insert/update/delete
   - Batch processing

**Beklenen Sonuç:** Performance optimization teknikleri uygulandı.

---

## ⚙️ Gelişmiş Application Katmanı Görevleri

### Task 9: Advanced DTOs and Mapping
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Gelişmiş DTO ve mapping sistemi

1. **Complex DTOs** oluşturun:
   - `TodoDetailDto` (tüm ilişkili verilerle)
   - `UserProfileDto` (kullanıcı istatistikleriyle)
   - `CategoryTreeDto` (hiyerarşik yapıyla)
   - `CommentThreadDto` (threaded yapıyla)

2. **Advanced AutoMapper** konfigürasyonu:
   - Custom value resolvers
   - Conditional mapping
   - Nested mapping
   - Custom type converters

3. **DTO Validation**:
   - Cross-field validation
   - Custom validation attributes
   - Async validation

**Beklenen Sonuç:** Gelişmiş DTO ve mapping sistemi oluşturuldu.

### Task 10: Advanced Service Layer
**Süre:** 180 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Gelişmiş service layer implementasyonu

1. **Business Logic Services**:
   - `TodoWorkflowService` (todo lifecycle management)
   - `UserPermissionService` (permission management)
   - `NotificationService` (notification management)
   - `AnalyticsService` (data analytics)

2. **Background Services**:
   - `RecurringTodoService` (recurring todo creation)
   - `CleanupService` (data cleanup)
   - `ReportGenerationService` (report generation)

3. **Integration Services**:
   - `EmailService` (email notifications)
   - `FileStorageService` (file management)
   - `ExternalApiService` (third-party integrations)

**Beklenen Sonuç:** Gelişmiş service layer implementasyonu tamamlandı.

### Task 11: Advanced Validation System
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** Gelişmiş validation sistemi

1. **Custom Validators**:
   - `UniqueEmailValidator`
   - `ValidCategoryHierarchyValidator`
   - `ValidTodoTemplateValidator`
   - `ValidRecurrencePatternValidator`

2. **Cross-Entity Validation**:
   - Todo-Category relationship validation
   - User-Permission validation
   - Comment-Thread validation

3. **Async Validation**:
   - Database-based validation
   - External API validation

**Beklenen Sonuç:** Gelişmiş validation sistemi oluşturuldu.

---

## 🌐 Gelişmiş API Katmanı Görevleri

### Task 12: Advanced Controllers
**Süre:** 150 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Gelişmiş controller implementasyonu

1. **Base Controller** oluşturun:
   - Common response handling
   - Error handling
   - Logging
   - Authentication/Authorization

2. **Advanced Endpoints**:
   - Bulk operations
   - Search and filtering
   - Export/Import functionality
   - File upload/download

3. **API Versioning**:
   - Multiple version support
   - Backward compatibility
   - Deprecation handling

**Beklenen Sonuç:** Gelişmiş controller implementasyonu tamamlandı.

### Task 13: Advanced Middleware
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Custom middleware implementasyonu

1. **Request/Response Logging**:
   - Detailed request logging
   - Response time tracking
   - Error logging

2. **Rate Limiting**:
   - User-based rate limiting
   - IP-based rate limiting
   - Endpoint-specific limits

3. **Security Middleware**:
   - CORS configuration
   - Security headers
   - Request validation

**Beklenen Sonuç:** Custom middleware implementasyonu tamamlandı.

### Task 14: Advanced Authentication/Authorization
**Süre:** 180 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Gelişmiş authentication/authorization sistemi

1. **JWT Authentication**:
   - Token generation/validation
   - Refresh token mechanism
   - Token blacklisting

2. **Role-Based Authorization**:
   - Custom authorization policies
   - Resource-based authorization
   - Permission-based authorization

3. **OAuth Integration**:
   - Google OAuth
   - Microsoft OAuth
   - GitHub OAuth

**Beklenen Sonuç:** Gelişmiş authentication/authorization sistemi oluşturuldu.

---

## 🔧 Gelişmiş Görevler

### Task 15: Caching Strategy
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Kapsamlı caching stratejisi

1. **Multi-Level Caching**:
   - In-memory caching
   - Redis caching
   - CDN caching

2. **Cache Invalidation**:
   - Event-based invalidation
   - Time-based expiration
   - Manual invalidation

3. **Cache Warming**:
   - Pre-loading strategies
   - Background cache warming

**Beklenen Sonuç:** Kapsamlı caching stratejisi uygulandı.

### Task 16: Advanced Testing
**Süre:** 200 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Gelişmiş test stratejisi

1. **Integration Tests**:
   - Database integration tests
   - API integration tests
   - End-to-end tests

2. **Performance Tests**:
   - Load testing
   - Stress testing
   - Memory profiling

3. **Security Tests**:
   - Penetration testing
   - Vulnerability scanning
   - Security code review

**Beklenen Sonuç:** Gelişmiş test stratejisi uygulandı.

### Task 17: Monitoring and Observability
**Süre:** 150 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Monitoring ve observability sistemi

1. **Application Monitoring**:
   - Health checks
   - Metrics collection
   - Performance monitoring

2. **Logging Strategy**:
   - Structured logging
   - Log aggregation
   - Log analysis

3. **Alerting System**:
   - Error alerting
   - Performance alerting
   - Business metric alerting

**Beklenen Sonuç:** Monitoring ve observability sistemi oluşturuldu.

---

## 🎨 Bonus Görevler

### Task 18: Microservices Architecture
**Süre:** 300 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Microservices architecture implementasyonu

1. **Service Decomposition**:
   - User Service
   - Todo Service
   - Notification Service
   - Analytics Service

2. **Inter-Service Communication**:
   - HTTP API communication
   - Message queuing
   - Event-driven architecture

3. **Service Discovery**:
   - Service registry
   - Load balancing
   - Circuit breaker pattern

### Task 19: Advanced File Management
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Gelişmiş dosya yönetim sistemi

1. **File Upload System**:
   - Multiple file formats
   - File validation
   - Virus scanning

2. **File Storage**:
   - Cloud storage integration
   - File compression
   - Thumbnail generation

3. **File Security**:
   - Access control
   - Encryption
   - Audit trail

### Task 20: Advanced Reporting System
**Süre:** 180 dakika  
**Zorluk:** ⭐⭐⭐⭐⭐

**Görev:** Gelişmiş raporlama sistemi

1. **Report Generation**:
   - Dynamic report generation
   - Multiple export formats
   - Scheduled reports

2. **Dashboard System**:
   - Real-time dashboards
   - Custom widgets
   - Data visualization

3. **Analytics Engine**:
   - User behavior analytics
   - Performance analytics
   - Business intelligence

---

## 📋 Görev Tamamlama Kriterleri

Her görev için aşağıdaki kriterleri sağlamanız gerekiyor:

### ✅ Kod Kalitesi
- [ ] Clean Architecture prensipleri uygulanmış
- [ ] SOLID prensipleri takip edilmiş
- [ ] Design patterns kullanılmış
- [ ] Performance optimization yapılmış
- [ ] Security best practices uygulanmış

### ✅ Test Edilebilirlik
- [ ] Unit test coverage %80+
- [ ] Integration test'ler yazılmış
- [ ] Performance test'ler yapılmış
- [ ] Security test'ler uygulanmış

### ✅ Dokümantasyon
- [ ] API dokümantasyonu güncel
- [ ] Database schema dokümante edilmiş
- [ ] Architecture decision records (ADR)
- [ ] Deployment guide hazırlanmış

---

## 🚨 Yardım ve Destek

### Sorun Yaşadığınızda:
1. **Hata mesajlarını** dikkatli okuyun
2. **Stack Overflow** ve **Microsoft Docs**'u kontrol edin
3. **GitHub Issues**'da benzer sorunları arayın


### Faydalı Kaynaklar:
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [Microservices Patterns](https://microservices.io/)
- [Performance Best Practices](https://docs.microsoft.com/en-us/aspnet/core/performance/)

---

## 🎯 Başarı Kriterleri

Bu görevleri tamamladıktan sonra:
- ✅ **Senior-level** N-Layer Architecture anlayışına sahip olacaksınız
- ✅ **Karmaşık** veritabanı ilişkilerini yönetebileceksiniz
- ✅ **Performance optimization** tekniklerini uygulayabileceksiniz
- ✅ **Advanced** .NET teknolojilerini kullanabileceksiniz
- ✅ **Enterprise-level** API tasarımı yapabileceksiniz
- ✅ **Microservices** architecture kavramlarını anlayacaksınız
- ✅ **Security** best practices uygulayabileceksiniz
- ✅ **Testing** stratejilerini geliştirebileceksiniz

**Başarılar! 🚀**