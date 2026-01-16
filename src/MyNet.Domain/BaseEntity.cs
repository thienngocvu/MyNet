using MyNet.Domain.Interfaces;

namespace MyNet.Domain
{
    /// <summary>
    /// Generic base entity class that implements IEntity and IAuditableEntity
    /// </summary>
    /// <typeparam name="TId">The type of the entity's identifier</typeparam>
    public abstract class BaseEntity<TId> : IEntity<TId>, IAuditableEntity
    {
        public virtual TId Id { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }

    /// <summary>
    /// Non-generic base entity class with Guid as the identifier type
    /// </summary>
    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }
}

