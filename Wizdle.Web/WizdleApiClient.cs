﻿namespace Wizdle.Web
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using Wizdle.Models;

    internal class WizdleApiClient(HttpClient httpClient)
    {
        public async Task<WizdleResponse> PostWizdleRequestAsync(WizdleRequest wizdleRequest, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("/", wizdleRequest, cancellationToken);

            return await httpResponseMessage.Content.ReadFromJsonAsync<WizdleResponse>(cancellationToken) ?? new WizdleResponse();
        }
    }
}