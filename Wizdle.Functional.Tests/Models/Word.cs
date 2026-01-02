namespace Wizdle.Functional.Tests.Models;

using System;
using System.Globalization;

internal sealed class Word
{
    private readonly string _value;
    private readonly LetterStatus[] _letterStatuses;

    public Word(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(value));
        }

        if (value.Length != 5)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Word must be 5 letters.");
        }

        _value = value;
        _letterStatuses = new LetterStatus[5];
    }

    public override string ToString() => _value;

    public char GetCharUpper(int index) => char.ToUpper(_value[index], CultureInfo.InvariantCulture);

    public void SetLetterStatus(int index, LetterStatus status)
    {
        _letterStatuses[index] = status;
    }

    public LetterStatus GetLetterStatus(int index)
    {
        return _letterStatuses[index];
    }
}
