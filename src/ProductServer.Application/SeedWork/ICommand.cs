using MediatR;

namespace ProductServer.Application.SeedWork
{
    public interface ICommand : IRequest<CommandResult> { }
}
