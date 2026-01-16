namespace MyNet.Domain.Entities
{
    /// <summary>
    /// Login log entity to track user login attempts
    /// </summary>
    public class LoginLog : BaseEntity<int>
    {
        public int UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public string RemoteAddress { get; set; } = null!;
        public short LoginResult { get; set; } // 0: Failed, 1: Success
        
        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
