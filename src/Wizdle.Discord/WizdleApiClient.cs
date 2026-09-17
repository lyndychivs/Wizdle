namespace Wizdle.Discord;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Wizdle.Models;

/// <summary>
/// A client for calling the Wizdle API.
/// </summary>
/// <param name="httpClient">The <see cref="HttpClient"/> to send requests with.</param>
public sealed class WizdleApiClient(HttpClient httpClient)
{
    /// <summary>
    /// Posts the given <see cref="WizdleRequest"/> to the Wizdle API.
    /// </summary>
    /// <param name="wizdleRequest">The request containing criteria for selecting words during the Solve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to cancel the operation with.</param>
    /// <returns>A task representing the asynchronous operation, containing the <see cref="WizdleResponse"/>.</returns>
    public async Task<WizdleResponse> PostWizdleRequestAsync(
        WizdleRequest wizdleRequest,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "/",
            wizdleRequest,
            cancellationToken);

        return await httpResponseMessage.Content.ReadFromJsonAsync<WizdleResponse>(cancellationToken)
            ?? new WizdleResponse();
    }
}
