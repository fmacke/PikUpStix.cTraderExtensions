using Application.Features.Tests.Commands.Create;
using Application.Features.Tests.Commands.Update;
using Application.Features.Tests.Queries.GetAllCached;
using Application.Features.Tests.Queries.GetAllPaged;
using Application.Features.Tests.Queries.GetById;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    internal class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<CreateTestCommand, Test>().ReverseMap();
            CreateMap<UpdateTestCommand, Test>().ReverseMap();            
            CreateMap<GetTestByIdResponse, Test>().ReverseMap();            
            CreateMap<GetAllTestsCachedResponse, Test>().ReverseMap();
            CreateMap<GetAllTestsResponse, Test>().ReverseMap();
        }
    }
}
