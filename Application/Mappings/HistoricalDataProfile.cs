using Application.Features.HistoricalDatas.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class HistoricalDataProfile : Profile
    {
        public HistoricalDataProfile()
        {
            CreateMap<CreateHistoricalDataCommand, HistoricalData>().ReverseMap();
        }
    }
}
