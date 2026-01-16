namespace MyNet.Domain.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        int? CreatedBy { get; set; }
        DateTime? LastModifiedAt { get; set; }
        int? LastModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        int? DeletedBy { get; set; }
    }
}
