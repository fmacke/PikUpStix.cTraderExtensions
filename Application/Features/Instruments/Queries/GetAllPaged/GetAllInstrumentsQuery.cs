using MediatR;
using System.Linq.Expressions;
using Application.Common.Results;
using Application.Interfaces.Repositories;
using Application.Common.Extensions;
using Domain.Entities;

namespace Application.Features.Instruments.Queries.GetAllPaged
{
    public class GetAllInstrumentsQuery : IRequest<PaginatedResult<GetAllInstrumentsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllInstrumentsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllInstrumentsQueryHandler : IRequestHandler<GetAllInstrumentsQuery, PaginatedResult<GetAllInstrumentsResponse>>
    {
        private readonly IInstrumentRepository _repository;

        public GetAllInstrumentsQueryHandler(IInstrumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<GetAllInstrumentsResponse>> Handle(GetAllInstrumentsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Instrument, GetAllInstrumentsResponse>> expression = e => new GetAllInstrumentsResponse
            {
                Id = e.Id
            };
            var paginatedList = await _repository.Instruments
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }
    }
}