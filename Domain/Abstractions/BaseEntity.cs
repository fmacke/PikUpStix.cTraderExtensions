namespace Domain.Abstractions
{
    public interface IBaseEntity
    {
        int Id { get; set; }
    }
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
