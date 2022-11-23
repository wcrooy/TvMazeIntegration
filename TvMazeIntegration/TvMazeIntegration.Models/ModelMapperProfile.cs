using AutoMapper;
using TvMazeIntegration.Models.Models;

namespace TvMazeIntegration.Models;

public class ModelMapperProfile:Profile
{
    public ModelMapperProfile()
    {
        CreateMap<ShowResponse, ShowResponseModel>();
        CreateMap<Show, ShowModel>();
        CreateMap<Actor, ActorModel>();
    }
}