namespace Application.Common.ThrowR
{
    public class Throw : IThrow
    {
        public static IThrow Exception { get; } = new Throw();
        private Throw() { }
    }
    public interface IThrow
    {
    }
}
