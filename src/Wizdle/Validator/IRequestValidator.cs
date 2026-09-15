namespace Wizdle.Validator;

using System.Collections.Generic;

using Wizdle.Models;

internal interface IRequestValidator
{
    IEnumerable<string> GetErrors(WizdleRequest request);
}
