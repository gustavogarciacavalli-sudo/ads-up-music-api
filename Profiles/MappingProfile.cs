using AutoMapper;
using BeatFlowApi.Models;
using BeatFlowApi.DTOs;

namespace BeatFlowApi.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Artist, ArtistDto>().ReverseMap();
        CreateMap<Track, TrackDto>().ReverseMap();
    }
}
