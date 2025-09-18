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

1. **User Entity** oluşturun:
   - `Id`, `FirstName`, `LastName`, `Email`, `PasswordHash`
   - `PhoneNumber`, `AvatarUrl`, `Bio`, `Location`
   - `CreatedAt`, `LastLoginAt`, `IsActive`, `IsEmailVerified`
   - `Role` (enum: SuperAdmin, Admin, Manager, User, Guest)

2. **UserProfile Entity** oluşturun:
   - `UserId`, `DateOfBirth`, `Gender`, `Interests`, `Skills`
   - `SocialMediaLinks`, `Preferences`

3. **UserSettings Entity** oluşturun:
   - `UserId`, `Theme`, `Language`, `Notifications`, `Privacy`

4. **UserActivity Entity** oluşturun:
   - `UserId`, `Action`, `Description`, `IpAddress`, `UserAgent`, `Timestamp`

**Beklenen Sonuç:** Kapsamlı kullanıcı yönetim sistemi oluşturuldu.

### Task 2: Advanced Category System
**Süre:** 90 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** Hiyerarşik kategori sistemi oluşturun

1. **Category Entity** güncelleyin:
   - `ParentCategoryId` (self-referencing foreign key)
   - `Level`, `Path`, `SortOrder`, `Icon`, `Color`
   - `IsSystemCategory`, `UsageCount`

2. **CategoryPermission Entity** oluşturun:
   - `CategoryId`, `UserId`, `PermissionType` (Read, Write, Delete, Admin)
   - `GrantedBy`, `GrantedAt`, `ExpiresAt`

3. **CategoryTag Entity** oluşturun:
   - `CategoryId`, `TagId` (many-to-many)

**Beklenen Sonuç:** Hiyerarşik ve izin tabanlı kategori sistemi oluşturuldu.

### Task 3: Advanced Tag System
**Süre:** 75 dakika  
**Zorluk:** ⭐⭐⭐

**Görev:** Gelişmiş etiket sistemi oluşturun

1. **Tag Entity** güncelleyin:
   - `ParentTagId` (self-referencing)
   - `Aliases`, `Synonyms`, `UsageCount`, `PopularityScore`
   - `IsSystemTag`, `Color`, `Icon`

2. **TagRelationship Entity** oluşturun:
   - `ParentTagId`, `ChildTagId`, `RelationshipType` (Synonym, Related, SubTag)

3. **TagUsage Entity** oluşturun:
   - `TagId`, `EntityType`, `EntityId`, `UsedBy`, `UsedAt`

**Beklenen Sonuç:** Gelişmiş etiket sistemi ve ilişkileri oluşturuldu.

### Task 4: Comment System with Threading
**Süre:** 120 dakika  
**Zorluk:** ⭐⭐⭐⭐

**Görev:** Threaded yorum sistemi oluşturun

1. **Comment Entity** güncelleyin:
   - `ParentCommentId` (self-referencing for threading)
   - `ThreadId`, `Level`, `Path`, `IsDeleted`, `DeletedAt`
   - `LikeCount`, `DislikeCount`, `ReportCount`

2. **CommentReaction Entity** oluşturun:
   - `CommentId`, `UserId`, `ReactionType` (Like, Dislike, Love, Angry)
   - `CreatedAt`

3. **CommentReport Entity** oluşturun:
   - `CommentId`, `ReportedBy`, `Reason`, `Description`, `Status`
   - `ReportedAt`, `ResolvedAt`, `ResolvedBy`

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

1. **Generic Repository** genişletme:
   - `IQueryable<T>` dönen metodlar
   - `Include` ve `ThenInclude` desteği
   - `OrderBy` ve `GroupBy` desteği

2. **Specification Pattern** implementasyonu:
   - `ISpecification<T>` interface
   - `BaseSpecification<T>` base class
   - `SpecificationEvaluator` class

3. **Unit of Work Pattern** implementasyonu:
   - `IUnitOfWork` interface
   - `UnitOfWork` implementation
   - Transaction management


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