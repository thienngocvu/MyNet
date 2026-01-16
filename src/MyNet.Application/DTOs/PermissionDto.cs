namespace MyNet.Application.DTOs
{
    public class PermissionDto
    {
        public string FunctionId { get; set; } = default!;
        public PermissionActionsDto Actions { get; set; } = new();
    }
    
    public class PermissionActionsDto
    {
        public bool List { get; set; }
        public bool Read { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Approve { get; set; }
        public bool Export { get; set; }
        public bool Import { get; set; }
    }
}
