namespace Wizdle.Web.Functional.Tests.Models;

using System;

internal interface IContainerHandle : IAsyncDisposable
{
    public Uri Url { get; }
}
