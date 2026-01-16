namespace MyNet.Domain.Entities
{
    public class LoginLog
    {
        public required string UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public required string RemoteAddress { get; set; }
        public short LoginResult { get;set; }
    }
}
