namespace Wizdle.Words
{
    using System.Collections.Generic;

    internal interface IWords
    {
        IEnumerable<string> GetWords();
    }
}
