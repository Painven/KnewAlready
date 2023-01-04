using AutoMapper;
using KnewAlreadyCore;

namespace KnewAlreadyWebApp.Data.MapperProfiles;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<UserDto, AppUser>();
        CreateMap<AppUser, UserDto>();

        CreateMap<UpdateUserDto, AppUser>();
        CreateMap<AppUser, UpdateUserDto>();

        CreateMap<CreateUserDto, AppUser>();
        CreateMap<AppUser, CreateUserDto>();
    }
}
