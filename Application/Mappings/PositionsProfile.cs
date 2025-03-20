using Application.Features.Positions.Commands.Update;
using Application.Features.TestTrades.Queries.GetAllCached;
using Application.Features.TestTrades.Queries.GetAllPaged;
using Application.Features.TestTrades.Queries.GetById;
using AutoMapper;
using Domain.Entities;
using Application.Features.Positions.Commands.Create;

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
