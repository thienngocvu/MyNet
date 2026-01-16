using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MyNet.Application.Interfaces.Persistence;
using MyNet.Infrastructure.Persistences;
using System.Dynamic;

namespace MyNet.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository Users { get; private set; }
        public ILoginLogRepository LoginLogs { get; private set; }
        public IFunctionRepository Functions { get; private set; }
        public IPermissionRepository Permissions { get; private set; }

        public UnitOfWork(AppDbContext context, 
            IUserRepository userRepository,
            ILoginLogRepository loginLogs,
            IFunctionRepository functionRepository,
            IPermissionRepository permissionRepository)
        {
            Context = context;
            LoginLogs = loginLogs;
            Users = userRepository;
            Functions = functionRepository;
            Permissions = permissionRepository;
        }

        public void BeginTransaction()
                => _transaction = Context.Database.BeginTransaction();

        public async Task CommitChangesAsync()
        {
            Context.ChangeTracker.DetectChanges();

            foreach (var entry in Context.ChangeTracker.Entries())
            {
                dynamic track = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    SetCreationProperties(track);
                    SetUpdateProperties(track);
                }

                if (entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                {
                    SetUpdateProperties(track);
                }
            }

            await Context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
        }

        private static object? HasProperty(dynamic item, string propertyName)
        {
            if (item is ExpandoObject eo)
            {
                return (eo as IDictionary<string, object>).ContainsKey(propertyName) ? "Found" : null;
            }
            else
            {
                return item.GetType().GetProperty(propertyName) ?? null;
            }
        }

        private void SetCreationProperties(dynamic track)
        {
            if (HasProperty(track, "CreatedDate") != null) track.CreatedDate = DateTime.UtcNow;
        }

        private void SetUpdateProperties(dynamic track)
        {
            if (HasProperty(track, "UpdatedDate") != null) track.UpdatedDate = DateTime.UtcNow;
        }

        public async Task<int> SaveChangesAsync()
        {
            Context.ChangeTracker.DetectChanges();

            foreach (var entry in Context.ChangeTracker.Entries())
            {
                dynamic track = entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    SetCreationProperties(track);
                    SetUpdateProperties(track);
                }

                if (entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                {
                    SetUpdateProperties(track);
                }
            }

            return await Context.SaveChangesAsync();
        }

        protected AppDbContext Context { get; private set; }
        
        private IDbContextTransaction? _transaction;
    }
}
