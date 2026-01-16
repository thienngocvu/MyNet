namespace MyNet.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ILoginLogRepository LoginLogs { get; }
        IFunctionRepository Functions { get; }
        IPermissionRepository Permissions { get; }
        
        void BeginTransaction();
        Task<int> SaveChangesAsync();
        Task CommitChangesAsync();
        void RollbackTransaction();
    }
}
