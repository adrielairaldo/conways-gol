namespace Conways.Service.Application.Abstractions;

/// <summary>
/// Represents a command that changes the state of the system.
/// </summary>
public interface ICommand<out TResult> { }