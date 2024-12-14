using Application.Features.TestParameters.Commands.Create;
using Application.Features.TestParameters.Commands.Update;
using Application.Features.TestParameters.Queries.GetAllCached;
using Application.Features.TestParameters.Queries.GetAllPaged;
using Application.Features.TestParameters.Queries.GetById;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    internal class TestParametersProfile : Profile
    {
        public TestParametersProfile()
        {
            CreateMap<CreateTestParameterCommand, Test_Parameter>().ReverseMap();
            CreateMap<UpdateTestParametersCommand, Test_Parameter>().ReverseMap();            
            CreateMap<GetTestParameterByIdResponse, Test_Parameter>().ReverseMap();            
            CreateMap<GetAllTestParametersCachedResponse, Test_Parameter>().ReverseMap();
            CreateMap<GetAllTestParametersResponse, Test_Parameter>().ReverseMap();
        }
    }
}
