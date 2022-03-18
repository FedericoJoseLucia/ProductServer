using MediatR;

namespace ProductServer.Application.SeedWork
{
    internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse> { }
}
