namespace Wizdle.Validator
{
    using System.Collections.Generic;

    internal class ValidatorResponse
    {
        public bool IsValid { get; set; } = true;

        public IList<string> Errors { get; init; } = [];
    }
}