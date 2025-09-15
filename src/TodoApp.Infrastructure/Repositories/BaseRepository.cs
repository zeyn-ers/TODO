using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

/// <summary>
/// Generic Base Repository Implementation
/// Bu sınıf, IRepository<T> interface'ini implement eder
/// Tüm entity'ler için ortak CRUD operasyonlarını sağlar
/// Repository Pattern'in temel implementasyonu
/// </summary>
/// <typeparam name="T">Entity tipi (örneğin: Todo, User, Product)</typeparam>
public class BaseRepository<T> : IRepository<T> where T : class
{
    /// <summary>
    /// Entity Framework DbContext - Veritabanı bağlantısı
    /// </summary>
    protected readonly TodoDbContext _context;
    
    /// <summary>
    /// Entity'ye özel DbSet - Veritabanı tablosu
    /// </summary>
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Constructor - Dependency Injection ile DbContext alır
    /// </summary>
    /// <param name="context">Entity Framework DbContext</param>
    public BaseRepository(TodoDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>(); // Generic entity için DbSet oluşturur
    }

    /// <summary>
    /// ID'ye göre tek bir entity getirir
    /// </summary>
    /// <param name="id">Aranacak entity'nin ID'si</param>
    /// <returns>Bulunan entity veya null</returns>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Tüm entity'leri getirir
    /// </summary>
    /// <returns>Entity listesi</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Belirli bir koşula göre entity'leri filtreler
    /// LINQ Expression kullanarak dinamik sorgular oluşturur
    /// </summary>
    /// <param name="predicate">Filtreleme koşulu (LINQ Expression)</param>
    /// <returns>Koşula uyan entity listesi</returns>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Yeni bir entity ekler
    /// Entity'yi veritabanına ekler ve değişiklikleri kaydeder
    /// </summary>
    /// <param name="entity">Eklenecek entity</param>
    /// <returns>Eklenen entity (ID ile birlikte)</returns>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity); // Entity'yi DbSet'e ekler
        await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydeder
        return entity;
    }

    /// <summary>
    /// Mevcut bir entity'yi günceller
    /// Entity'nin durumunu "Modified" olarak işaretler ve kaydeder
    /// </summary>
    /// <param name="entity">Güncellenecek entity</param>
    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity); // Entity'yi güncellenmiş olarak işaretler
        await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydeder
    }

    /// <summary>
    /// ID'ye göre entity'yi siler
    /// Önce entity'yi bulur, sonra siler
    /// </summary>
    /// <param name="id">Silinecek entity'nin ID'si</param>
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id); // Entity'yi bul
        if (entity != null)
        {
            _dbSet.Remove(entity); // Entity'yi sil
            await _context.SaveChangesAsync(); // Değişiklikleri kaydet
        }
    }

    /// <summary>
    /// Belirli bir ID'ye sahip entity'nin var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="id">Kontrol edilecek ID</param>
    /// <returns>Entity varsa true, yoksa false</returns>
    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}
