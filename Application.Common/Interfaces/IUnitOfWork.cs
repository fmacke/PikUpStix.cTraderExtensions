namespace Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit(CancellationToken cancellationToken);
        Task Rollback();
    }
}
