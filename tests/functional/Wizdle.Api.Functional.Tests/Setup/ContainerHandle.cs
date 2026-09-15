namespace Wizdle.Api.Functional.Tests.Setup;

using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;

internal sealed class ContainerHandle : IAsyncDisposable
{
    private readonly IContainer _container;

    internal ContainerHandle(Uri url, IContainer container)
    {
        Url = url ?? throw new ArgumentNullException(nameof(url));
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public Uri Url { get; private set; }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
