namespace Application.Common.Interfaces.Shared
{
    public interface IAuthenticatedUserService
    {
        string UserId { get; }
        public string Username { get; }
    }
}