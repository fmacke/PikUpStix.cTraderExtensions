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
            CreateMap<CreateTestTradeCommand, Test_Parameter>().ReverseMap();
            CreateMap<UpdateTestTradesCommand, Test_Parameter>().ReverseMap();            
            CreateMap<GetTestTradeByIdResponse, Test_Parameter>().ReverseMap();            
            CreateMap<GetAllTestTradesCachedResponse, Test_Parameter>().ReverseMap();
            CreateMap<GetAllTestTradesResponse, Test_Parameter>().ReverseMap();
        }
    }
}
