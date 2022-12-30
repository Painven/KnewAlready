using AutoMapper;
using KnewAlreadyCore;

namespace KnewAlreadyWebApp.Data.MapperProfiles;

public class SuggestActionModelProfile : Profile
{
    public SuggestActionModelProfile()
    {
        CreateMap<SuggestActionModel, SuggestActionItemDto>();
        CreateMap<SuggestActionItemDto, SuggestActionModel>()
            .ForMember(x => x.Created, x => x.MapFrom(p => p.Created.UtcDateTime));
    }
}
