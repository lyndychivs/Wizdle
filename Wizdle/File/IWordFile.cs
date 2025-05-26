namespace Wizdle.File
{
    using System.Collections.Generic;

    internal interface IWordFile
    {
        IEnumerable<string> ReadLines();
    }
}