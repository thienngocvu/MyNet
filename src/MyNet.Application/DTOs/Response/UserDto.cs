using MyNet.Domain.Entities;
using System.Linq.Expressions;

namespace MyNet.Application.DTOs.Response
{
    public class UserDto
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Roles { get; set; } = [];

        public static Expression<Func<User, UserDto>> Projection
        {
            get
            {
                return entity => new UserDto
                {
                    UserId = entity.Id.ToString(),
                    Name = entity.UserName,
                    Email = entity.Email,
                    IsActive = entity.IsActive
                };
            }
        }

        public static UserDto? Create(User entity)
        {
            if (entity == null)
                return null;
            return Projection.Compile().Invoke(entity);
        }
    }
}
