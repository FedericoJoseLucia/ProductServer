using MediatR;

namespace ProductServer.Application.SeedWork
{
    public interface IQuery<out TResponse> : IRequest<TResponse> { }
}
