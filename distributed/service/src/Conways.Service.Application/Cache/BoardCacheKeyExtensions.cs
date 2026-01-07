using Conways.Service.Domain.Boards;

namespace Conways.Service.Application.Cache;

/// <summary>
/// Provides extension methods for converting <see cref="BoardId"/> values
/// into cache keys used by the application caching layer.
/// </summary>
/// <remarks>
/// This class centralizes cache key generation to ensure consistency
/// across the application and to avoid hard-coded key formats
/// in handlers or services.
/// 
/// The cache key format is intentionally deterministic and stable,
/// allowing safe cache invalidation and retrieval.
/// </remarks>
public static class BoardCacheKeyExtensions
{
    /// <summary>
    /// Converts the specified <see cref="BoardId"/> into a cache key string.
    /// </summary>
    /// <param name="boardId">The board identifier.</param>
    /// <returns>
    /// A cache key string uniquely identifying the board in the cache.
    /// </returns>
    /// <example>
    /// <code>
    /// var cacheKey = boardId.ToCacheKey();
    /// </code>
    /// </example>
    public static string ToCacheKey(this BoardId boardId) => $"boards:{boardId.Value}";
}