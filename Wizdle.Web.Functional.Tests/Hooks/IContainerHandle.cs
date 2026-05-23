namespace Wizdle.Web.Functional.Tests.Hooks;

using System;

internal interface IContainerHandle : IAsyncDisposable
{
    public Uri Url { get; }
}
