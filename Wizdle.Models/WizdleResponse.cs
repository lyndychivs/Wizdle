namespace Wizdle.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The response model for the Wizdle library.
    /// </summary>
    public class WizdleResponse
    {
        /// <summary>
        /// Gets response messages containing information about the request processed.
        /// </summary>
        public IEnumerable<string> Messages { get; init; } = [];

        /// <summary>
        /// Gets a collection of words that match the criteria specified in the request.
        /// </summary>
        public IEnumerable<string> Words { get; init; } = [];
    }
}