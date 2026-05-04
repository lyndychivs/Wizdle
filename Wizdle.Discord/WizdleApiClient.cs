namespace Wizdle.Discord;

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using Wizdle.Models;

internal sealed class WizdleApiClient(HttpClient httpClient, IMemoryCache memoryCache)
{
    private static readonly TimeSpan CacheExpiry = TimeSpan.FromHours(1);

    private readonly ConcurrentDictionary<string, SemaphoreSlim> _keyLocks = new(StringComparer.Ordinal);

    public async Task<WizdleResponse> PostWizdleRequestAsync(WizdleRequest wizdleRequest, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"{wizdleRequest.CorrectLetters.Length}:{wizdleRequest.CorrectLetters}|{wizdleRequest.MisplacedLetters.Length}:{wizdleRequest.MisplacedLetters}|{wizdleRequest.ExcludeLetters.Length}:{wizdleRequest.ExcludeLetters}";

        if (memoryCache.TryGetValue(cacheKey, out WizdleResponse? cached) && cached is not null)
        {
            return cached;
        }

        SemaphoreSlim keyLock = _keyLocks.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));
        await keyLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (memoryCache.TryGetValue(cacheKey, out cached)
                && cached is not null)
            {
                return cached;
            }

            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("/", wizdleRequest, cancellationToken).ConfigureAwait(false);

            WizdleResponse response = await httpResponseMessage.Content.ReadFromJsonAsync<WizdleResponse>(cancellationToken).ConfigureAwait(false)
                ?? new WizdleResponse();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheExpiry)
                .SetSize(1)
                .RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    if (key is string k
                        && _keyLocks.TryRemove(k, out SemaphoreSlim? semaphore))
                    {
                        semaphore.Dispose();
                    }
                });

            memoryCache.Set(cacheKey, response, cacheEntryOptions);

            return response;
        }
        finally
        {
            keyLock.Release();
        }
    }
}
