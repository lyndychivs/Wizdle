namespace Wizdle.Validator
{
    using System.Collections.Generic;

    internal class ValidatorResponse
    {
        public bool IsValid { get; set; } = true;

        public List<string> Errors { get; set; } = new List<string>();
    }
}