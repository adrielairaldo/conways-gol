namespace Conways.Service.Application.Abstractions;

/// <summary>
/// Represents a query to retrieve data without changing system state.
/// </summary>
public interface IQuery<out TResult> { }