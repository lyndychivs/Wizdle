namespace Wizdle.Validator;

using System.Collections.Generic;

using Wizdle.Models;

/// <summary>
/// Validates a <see cref="WizdleRequest"/>.
/// </summary>
internal interface IRequestValidator
{
    /// <summary>
    /// Gets the validation errors for the given <see cref="WizdleRequest"/>.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation errors, or an empty collection if none.</returns>
    IEnumerable<string> GetErrors(WizdleRequest request);
}
