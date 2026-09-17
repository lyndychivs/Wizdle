namespace Wizdle.Repository;

using System.Collections.Generic;

/// <summary>
/// Provides access to the collection of words used for solving.
/// </summary>
internal interface IWordRepository
{
    /// <summary>
    /// Gets the collection of valid words.
    /// </summary>
    /// <returns>The collection of valid words.</returns>
    IEnumerable<string> GetWords();
}
