namespace Wizdle.Tests.Validator;

using NUnit.Framework;

using Wizdle.Validator;

[TestFixture]
public class ValidatorResponseTests
{
    [Test]
    public void Constructor_WithNoParameters_ReturnsDefaultValidatorResponse()
    {
        var validatorResponse = new ValidatorResponse();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(validatorResponse.IsValid, Is.True);
            Assert.That(validatorResponse.Errors, Is.Empty);
        }
    }

    [Test]
    public void Constructor_WithIsValidSetFalse_ReturnsValidatorResponseWithFalse()
    {
        var response = new ValidatorResponse
        {
            IsValid = false,
        };

        Assert.That(response.IsValid, Is.False);
    }

    [Test]
    public void Constructor_WithErrors_ReturnsValidatorResponseWithErrors()
    {
        var validatorResponse = new ValidatorResponse
        {
            Errors = ["Error1", "Error2"],
        };

        Assert.That(validatorResponse.Errors, Is.EqualTo(["Error1", "Error2"]));
    }
}