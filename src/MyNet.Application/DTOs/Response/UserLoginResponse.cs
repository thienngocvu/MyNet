using MyNet.Domain.Entities;
using System.Linq.Expressions;

namespace MyNet.Application.DTOs.Response
{
    public class UserLoginResponse
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }

        public static Expression<Func<User, UserLoginResponse>> Projection
        {
            get
            {
                return entity => new UserLoginResponse
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    Email = entity.Email,
                    IsActive = entity.IsActive
                };
            }
        }

        public static UserLoginResponse? Create(User entity)
        {
            if (entity == null)
                return null;
            return Projection.Compile().Invoke(entity);
        }
    }
}
