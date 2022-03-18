using MediatR;

namespace ProductServer.Application.SeedWork
{
    internal interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, CommandResult>
        where TCommand : ICommand { }
}
