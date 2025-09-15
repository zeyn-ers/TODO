using System.Linq.Expressions;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Generic Repository Pattern Interface
/// Bu interface, tüm entity'ler için temel CRUD operasyonlarını tanımlar
/// Repository Pattern, veri erişim mantığını soyutlar ve test edilebilirliği artırır
/// </summary>
/// <typeparam name="T">Entity tipi (örneğin: Todo, User, Product)</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// ID'ye göre tek bir entity getirir
    /// </summary>
    /// <param name="id">Aranacak entity'nin ID'si</param>
    /// <returns>Bulunan entity veya null</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Tüm entity'leri getirir
    /// </summary>
    /// <returns>Entity listesi</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Belirli bir koşula göre entity'leri filtreler
    /// </summary>
    /// <param name="predicate">Filtreleme koşulu (LINQ Expression)</param>
    /// <returns>Koşula uyan entity listesi</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Yeni bir entity ekler
    /// </summary>
    /// <param name="entity">Eklenecek entity</param>
    /// <returns>Eklenen entity (ID ile birlikte)</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Mevcut bir entity'yi günceller
    /// </summary>
    /// <param name="entity">Güncellenecek entity</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// ID'ye göre entity'yi siler
    /// </summary>
    /// <param name="id">Silinecek entity'nin ID'si</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Belirli bir ID'ye sahip entity'nin var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="id">Kontrol edilecek ID</param>
    /// <returns>Entity varsa true, yoksa false</returns>
    Task<bool> ExistsAsync(int id);
}
