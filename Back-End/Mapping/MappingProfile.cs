using AutoMapper;
using WeatherLens.Models;
using WeatherLens.DTOs;

namespace WeatherLens.Mapping;

/// <summary>
/// Configures mappings between EF Core entities and API DTOs.
/// </summary>
public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserRequest, User>();
        CreateMap<User, UserResult>();

        // Location
        CreateMap<LocationRequest, Location>();
        CreateMap<Location, LocationResult>();

        // Weather Variable
        CreateMap<WeatherVariableRequest, WeatherVariable>();
        CreateMap<WeatherVariable, WeatherVariableResult>();

        // Weather Query
        CreateMap<WeatherQueryRequest, WeatherQuery>();
        CreateMap<WeatherQuery, WeatherQueryResult>();

        // Weather Query Variable
        CreateMap<WeatherQueryVariableRequest, WeatherQueryVariable>();
        CreateMap<WeatherQueryVariable, WeatherQueryVariableResult>();

        // Weather Result
        CreateMap<WeatherResultRequest, WeatherResult>();
        CreateMap<WeatherResult, WeatherResultResult>();
    }
}
