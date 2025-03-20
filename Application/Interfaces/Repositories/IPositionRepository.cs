using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPositionRepository
    {
        IQueryable<Position> Positions { get; }
        Task<List<Position>> GetListAsync();
        Task<Position> GetByIdAsync(int positionId);
        Task<int> InsertAsync(Position position);
        Task<int> InsertRangeAsync(List<Position> positions);
        Task UpdateAsync(Position position);
        Task DeleteAsync(Position position);
    }
}
