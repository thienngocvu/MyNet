using MyNet.Domain.Interfaces;

namespace MyNet.Application.Interfaces.Persistence
{
    public interface IRepository<TEntity, TId> where TEntity : IEntity<TId>
    {
        IQueryable<TEntity> AsQueryable();
        Task AddAsync(TEntity e);
        void Add(TEntity e);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity?> FindAsync(TId id);
        void Remove(TEntity e);
        void RemoveRange(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task<TEntity> UpdateAsync(TId key, TEntity e);
        Task<IEnumerable<TEntity>> AddOrUpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}
