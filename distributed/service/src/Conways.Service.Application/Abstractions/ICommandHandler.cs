namespace Conways.Service.Application.Abstractions;

/// <summary>
/// Defines a contract for handling a specific command.
/// </summary>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}