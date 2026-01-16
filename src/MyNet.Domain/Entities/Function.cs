namespace MyNet.Domain.Entities
{
    public class Function : BaseEntity<string>
    {
        public string Code { get; set; } = null!; // SYSTEM, USERS, ROLES...

        public string Name { get; set; } = null!;

        public string? ParentId { get; set; }

        public int SortOrder { get; set; }

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public virtual Function? Parent { get; set; }
        public virtual ICollection<Function> Children { get; set; } = [];
        public virtual ICollection<Permission> Permissions { get; set; } = [];
    }
}
