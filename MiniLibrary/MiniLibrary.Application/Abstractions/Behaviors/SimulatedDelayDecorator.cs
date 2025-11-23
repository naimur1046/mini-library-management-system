using MiniLibrary.Application.Abstractions.Messaging;
using MiniLibrary.SharedKernel;

namespace MiniLibrary.Application.Abstractions.Behaviors;

internal static class SimulatedDelayDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            return await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            return await innerHandler.Handle(command, cancellationToken);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            return await innerHandler.Handle(query, cancellationToken);
        }
    }
}
