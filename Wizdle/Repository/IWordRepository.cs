namespace Wizdle.Repository
{
    using System.Collections.Generic;

    internal interface IWordRepository
    {
        IEnumerable<string> GetWords();
    }
}