namespace Wizdle.Discord;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using Wizdle.Models;

internal sealed class WizdleApiClient(HttpClient httpClient, IMemoryCache memoryCache)
{
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromHours(1);

    public async Task<WizdleResponse> PostWizdleRequestAsync(WizdleRequest wizdleRequest, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"{wizdleRequest.CorrectLetters}|{wizdleRequest.MisplacedLetters}|{wizdleRequest.ExcludeLetters}";

        if (memoryCache.TryGetValue(cacheKey, out WizdleResponse? cached)
            && cached is not null)
        {
            return cached;
        }

        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("/", wizdleRequest, cancellationToken).ConfigureAwait(false);

        WizdleResponse response = await httpResponseMessage.Content.ReadFromJsonAsync<WizdleResponse>(cancellationToken).ConfigureAwait(false)
            ?? new WizdleResponse();

        memoryCache.Set(cacheKey, response, CacheExpiry);

        return response;
    }
}
