namespace Wizdle.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The response model for the Wizdle library.
    /// </summary>
    public class WizdleResponse
    {
        /// <summary>
        /// A response message containing information about the request processed.
        /// </summary>
        public IEnumerable<string> Message { get; init; } = [];

        /// <summary>
        /// A collection of words that match the criteria specified in the request.
        /// </summary>
        public IEnumerable<string> Words { get; init; } = [];
    }
}