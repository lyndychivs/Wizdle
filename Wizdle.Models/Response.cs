namespace Wizdle.Models
{
    using System.Collections.Generic;

    public class Response
    {
        public IEnumerable<string> Message { get; init; } = [];

        public IEnumerable<string> Words { get; init; } = [];
    }
}