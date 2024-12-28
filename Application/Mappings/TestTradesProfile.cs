using Application.Features.TestTrades.Commands.Create;
using Application.Features.TestTrades.Commands.Update;
using Application.Features.TestTrades.Queries.GetAllCached;
using Application.Features.TestTrades.Queries.GetAllPaged;
using Application.Features.TestTrades.Queries.GetById;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings 
{
    internal class TestTradesProfile : Profile
    {
        public TestTradesProfile()
        {
            CreateMap<CreateTestTradeCommand, TestTrade>().ReverseMap();
            CreateMap<UpdateTestTradesCommand, TestTrade>().ReverseMap();            
            CreateMap<GetTestTradeByIdResponse, TestTrade>().ReverseMap();            
            CreateMap<GetAllTestTradesCachedResponse, TestTrade>().ReverseMap();
            CreateMap<GetAllTestTradesResponse, TestTrade>().ReverseMap();
        }
    }
}
