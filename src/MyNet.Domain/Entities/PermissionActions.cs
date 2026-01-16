namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Represents the available actions/permissions for a function
    /// Stored as JSONB in PostgreSQL
    /// </summary>
    public class PermissionActions
    {
        public bool List { get; set; }
        public bool Read { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Approve { get; set; }
        public bool Export { get; set; }
        public bool Import { get; set; }
        
        /// <summary>
        /// Creates a PermissionActions with all permissions set to false
        /// </summary>
        public static PermissionActions None => new();
        
        /// <summary>
        /// Creates a PermissionActions with all permissions set to true
        /// </summary>
        public static PermissionActions All => new()
        {
            List = true,
            Read = true,
            Create = true,
            Update = true,
            Delete = true,
            Approve = true,
            Export = true,
            Import = true
        };
        
        /// <summary>
        /// Creates a PermissionActions with basic CRUD permissions
        /// </summary>
        public static PermissionActions Crud => new()
        {
            List = true,
            Read = true,
            Create = true,
            Update = true,
            Delete = true
        };
        
        /// <summary>
        /// Creates a PermissionActions with read-only permissions
        /// </summary>
        public static PermissionActions ReadOnly => new()
        {
            List = true,
            Read = true
        };
    }
}
