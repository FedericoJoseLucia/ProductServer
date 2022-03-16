namespace ProductServer.Application.SeedWork
{
    public interface ICommandResult
    {
        bool IsSuccess { get; }
        string Message { get; }
    }
}
