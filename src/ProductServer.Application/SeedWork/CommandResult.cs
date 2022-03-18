namespace ProductServer.Application.SeedWork
{
    public class CommandResult
    {
        public static CommandResult Success() => new(true, Enumerable.Empty<string>());
        public static CommandResult Error(string error) => new(false, new[] { error });
        public static CommandResult Error(IEnumerable<string> errors) => new(false, errors);

        private CommandResult(bool isSuccess, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; init; }
        public IEnumerable<string> Errors { get; init; }
    }
}
