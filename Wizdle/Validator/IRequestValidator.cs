namespace Wizdle.Validator;

using Wizdle.Models;

internal interface IRequestValidator
{
    ValidatorResponse IsValid(WizdleRequest request);
}
