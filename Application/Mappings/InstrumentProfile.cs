using Application.Features.Instruments.Commands.Create;
using Application.Features.Instruments.Commands.Update;
using Application.Features.Instruments.Queries.GetAllCached;
using Application.Features.Instruments.Queries.GetAllPaged;
using Application.Features.Instruments.Queries.GetById;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class InstrumentProfile : Profile
    {
        public InstrumentProfile()
        {
            CreateMap<CreateInstrumentCommand, Instrument>().ReverseMap();
            CreateMap<UpdateInstrumentCommand, Instrument>().ReverseMap();            
            CreateMap<GetInstrumentByIdResponse, Instrument>().ReverseMap();            
            CreateMap<GetAllInstrumentsCachedResponse, Instrument>().ReverseMap();
            CreateMap<GetAllInstrumentsResponse, Instrument>().ReverseMap();
        }
    }
}
