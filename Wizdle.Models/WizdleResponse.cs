namespace Wizdle.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The response model for the Wizdle library.
    /// </summary>
    public class WizdleResponse
    {
        /// <summary>
        /// Gets or sets response messages containing information about the request processed.
        /// </summary>
        public IEnumerable<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a collection of words that match the criteria specified in the request.
        /// </summary>
        public IEnumerable<string> Words { get; set; } = new List<string>();
    }
}