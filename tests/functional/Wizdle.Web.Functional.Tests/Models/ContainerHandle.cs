namespace Wizdle.Web.Functional.Tests.Models;

using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

internal sealed class ContainerHandle : IContainerHandle
{
    private readonly IContainer _apiContainer;
    private readonly IContainer _webContainer;

    private readonly INetwork _network;

    internal ContainerHandle(Uri url, IContainer apiContainer, IContainer webContainer, INetwork network)
    {
        Url = url ?? throw new ArgumentNullException(nameof(url));

        _apiContainer = apiContainer ?? throw new ArgumentNullException(nameof(apiContainer));
        _webContainer = webContainer ?? throw new ArgumentNullException(nameof(webContainer));

        _network = network ?? throw new ArgumentNullException(nameof(network));
    }

    public Uri Url { get; private set; }

    public async ValueTask DisposeAsync()
    {
        await _webContainer.DisposeAsync();
        await _apiContainer.DisposeAsync();
        await _network.DisposeAsync();
    }
}
