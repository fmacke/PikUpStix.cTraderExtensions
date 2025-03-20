using Application.Features.Positions.Commands.Update;
using AutoMapper;
using Domain.Entities;
using Application.Features.Positions.Commands.Create;
using Application.Features.Positions.Queries.GetAllCached;
using Application.Features.Positions.Queries.GetAllPaged;
using Application.Features.Positions.Queries.GetById;

namespace Application.Mappings 
{
    public class PositionsProfile : Profile
    {
        public PositionsProfile()
        {
            CreateMap<CreatePositionCommand, Position>().ReverseMap();
            CreateMap<UpdatePositionsCommand, Position>().ReverseMap();            
            CreateMap<GetPositionByIdResponse, Position>().ReverseMap();            
            CreateMap<GetAllPositionsCachedResponse, Position>().ReverseMap();
            CreateMap<GetAllPositionsResponse, Position>().ReverseMap();
        }
    }
}
