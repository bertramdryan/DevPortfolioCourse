using AutoMapper;
using Shared.Models;

namespace Server.Data;

internal sealed class DtoMappings : Profile
{
    public DtoMappings()
    {
        CreateMap<Post, PostDto>().ReverseMap();
    }
}

