namespace Conways.Service.Application.Abstractions;

/// <summary>
/// Defines a contract for handling a specific query.
/// </summary>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery command, CancellationToken cancellationToken);
}