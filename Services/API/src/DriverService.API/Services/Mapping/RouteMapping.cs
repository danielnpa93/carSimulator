using AutoMapper;
using DriverService.API.Domain.DTO;
using DriverService.API.Domain.Entities;
using Simulator.Schema;
using System.Globalization;
using Route = DriverService.API.Domain.Entities.Route;

namespace DriverService.API.Services.Mapping
{
    public class RouteMapping : Profile
    {
        public RouteMapping()
        {
            CreateMap<CreateRouteDTO, Route>()
               .ConstructUsing(src => new Route(src.Title, src.StartPosition, src.EndPosition));

            CreateMap<UpdateRouteDTO, Route>()
                .ConstructUsing(src => new Route(src.Title, src.StartPosition, src.EndPosition));

            CreateMap<Route, InitRouteModel>()
                .ForMember(dest => dest.EndRoute, opt => opt.MapFrom(src => src.EndPosition))
                .ForMember(dest => dest.StartRoute, opt => opt.MapFrom(src => src.StartPosition))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id));
    

            CreateMap<Position, Simulator.Schema.Route>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude.ToString(CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude.ToString(CultureInfo.InvariantCulture)));

        }
    }
}
