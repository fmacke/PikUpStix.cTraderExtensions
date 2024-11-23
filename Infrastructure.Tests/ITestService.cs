using Application.Common.Results;
using Application.Features.Tests.Queries.GetAllCached;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITestService { 
    Task<Result<List<GetAllTestsCachedResponse>>> GetAllTestsCachedAsync(); 
}
public class TestService : ITestService
{
    private readonly IMediator _mediator;

    public TestService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<List<GetAllTestsCachedResponse>>> GetAllTestsCachedAsync()
    {
        var query = new GetAllTestsCachedQuery();
        return await _mediator.Send(query);
    }
}
