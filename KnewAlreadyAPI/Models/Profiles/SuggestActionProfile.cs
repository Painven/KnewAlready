using AutoMapper;
using KnewAlreadyAPI.DataAccess.Entities;
using KnewAlreadyAPI.Dtos;

namespace KnewAlreadyAPI.Models.Profiles;

public class SuggestActionProfile : Profile
{
    public SuggestActionProfile()
    {
        CreateMap<SuggestActionItemDto, SuggestActionItem>();
        CreateMap<SuggestActionItem, SuggestActionItemDto>();
    }
}
