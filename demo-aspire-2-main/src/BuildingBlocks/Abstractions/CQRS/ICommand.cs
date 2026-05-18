using MediatR;

namespace BuildingBlocks.Abstractions.CQRS;

/// <summary>Marker cho Command không trả về giá trị.</summary>
public interface ICommand : IRequest { }

/// <summary>Marker cho Command trả về <typeparamref name="TResponse"/>.</summary>
public interface ICommand<out TResponse> : IRequest<TResponse> { }

/// <summary>Handler cho ICommand.</summary>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand { }

/// <summary>Handler cho ICommand&lt;TResponse&gt;.</summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse> { }
