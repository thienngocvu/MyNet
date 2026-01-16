namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Permission entity representing the actions a role can perform on a function
    /// </summary>
    public class Permission : BaseEntity<int>
    {
        public int RoleId { get; set; }
        
        public string FunctionId { get; set; } = null!;
        
        /// <summary>
        /// Actions allowed for this role on this function (stored as JSONB)
        /// Example: {"list": true, "read": true, "create": true, "update": true, "delete": false}
        /// </summary>
        public PermissionActions Actions { get; set; } = new();

        public virtual Role Role { get; set; } = null!;
        public virtual Function Function { get; set; } = null!;
    }
}
