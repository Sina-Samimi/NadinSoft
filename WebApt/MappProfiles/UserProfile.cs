using Application.DTOs.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace WebApi.MappProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, UserRegisterDto>()
                .ForMember(p=>p.Password,opt=>opt.MapFrom(src=>src.PasswordHash))
               .ReverseMap();
            CreateMap<IdentityUser, UserLogin>()
                .ReverseMap();
        }
    }
}
