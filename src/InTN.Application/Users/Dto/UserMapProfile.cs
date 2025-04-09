using InTN.Authorization.Users;
using AutoMapper;

namespace InTN.Users.Dto;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore());

        CreateMap<CreateUserDto, User>();
        CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
    }
}
