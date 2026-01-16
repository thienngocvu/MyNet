using Microsoft.EntityFrameworkCore;
using MyNet.Domain;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Infrastructure.Persistences;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    public abstract class GenericRepository<TEntity, TId> : IRepository<TEntity, TId>
                                                where TEntity : BaseEntity<TId>
    {
        protected AppDbContext _dbContext { get; private set; }
        protected DbSet<TEntity> _dbSet;

        protected GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> AsQueryable() => _dbSet.AsQueryable();

        public virtual async Task AddAsync(TEntity e) => await _dbSet.AddAsync(e);
        
        public virtual void Add(TEntity e) => _dbSet.Add(e);
        
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

        public virtual async Task<TEntity?> FindAsync(TId id) => await _dbSet.FindAsync(id);

        public virtual void Remove(TEntity e) => _dbSet.Remove(e);

        public virtual async Task<TEntity> UpdateAsync(TId key, TEntity e)
        {
            var trackingEntity = await FindAsync(key);
            if (trackingEntity != null)
            {
                Update(e, trackingEntity);
            }
            return e;
        }

        protected abstract void Update(TEntity requestObject, TEntity targetObject);

        public virtual void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        public void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

        public async Task<IEnumerable<TEntity>> AddOrUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            var ids = entities.Select(x => x.Id);
            var listExists = await _dbSet.AsQueryable().Where(x => ids.Any(z => z.Equals(x.Id))).ToListAsync();
            var existIds = listExists.Select(x => x.Id);
            var listNotExists = entities.Where(x => !existIds.Any(z => z.Equals(x.Id))).ToList();

            if (listExists.Any())
            {
                foreach (var item in listExists)
                {
                    Update(entities.FirstOrDefault(x => x.Id.Equals(item.Id))!, item);
                }
            }

            if (listNotExists.Any())
            {
                await _dbSet.AddRangeAsync(listNotExists);
            }

            return entities;
        }
    }
}
