namespace Wizdle.Words;

using System.Collections.Generic;

/// <summary>
/// Provides a source of words.
/// </summary>
internal interface IWords
{
    /// <summary>
    /// Gets the collection of words.
    /// </summary>
    /// <returns>The collection of words.</returns>
    IEnumerable<string> GetWords();
}
