using MediatR;

namespace BuildingBlocks.Abstractions.CQRS;

/// <summary>Marker cho Query trả về <typeparamref name="TResponse"/>.</summary>
public interface IQuery<out TResponse> : IRequest<TResponse> { }

/// <summary>Handler cho IQuery&lt;TResponse&gt;.</summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse> { }
