using AutoMapper;
using MyNet.Domain.Entities;
using MyNet.Application.DTOs.Request;
using MyNet.Application.DTOs.Response;

namespace MyNet.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, CreateUserRequest>();
            CreateMap<User, CreateUserResponse>();
        }
    }
}
