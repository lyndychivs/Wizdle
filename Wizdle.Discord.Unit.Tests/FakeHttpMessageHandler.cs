namespace Wizdle.Discord.Unit.Tests;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpResponseMessage> _responseFactory;
    private readonly Action<HttpRequestMessage>? _onRequest;

    public FakeHttpMessageHandler(
    HttpResponseMessage template,
    Action<HttpRequestMessage>? onRequest = null)
    : this(CreateResponseFactory(template), onRequest)
    {
    }

    public FakeHttpMessageHandler(
        Func<HttpResponseMessage> responseFactory,
        Action<HttpRequestMessage>? onRequest = null)
    {
        _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
        _onRequest = onRequest;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _onRequest?.Invoke(request);
        return Task.FromResult(_responseFactory());
    }

    private static Func<HttpResponseMessage> CreateResponseFactory(HttpResponseMessage template)
    {
        return () => new HttpResponseMessage(template.StatusCode)
        {
            Content = CloneContent(template.Content),
            ReasonPhrase = template.ReasonPhrase,
            Version = template.Version,
        };
    }

    private static HttpContent? CloneContent(HttpContent? content)
    {
        if (content is null)
        {
            return null;
        }

        if (content is StringContent)
        {
            string contentString = content.ReadAsStringAsync().GetAwaiter().GetResult();
            System.Text.Encoding encoding = content.Headers.ContentType?.CharSet is not null
                ? System.Text.Encoding.GetEncoding(content.Headers.ContentType.CharSet)
                : System.Text.Encoding.UTF8;
            StringContent cloned = new(contentString, encoding);

            if (content.Headers.ContentType is not null)
            {
                cloned.Headers.ContentType = content.Headers.ContentType;
            }

            return cloned;
        }

        if (content is ByteArrayContent)
        {
            byte[] contentBytes = content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            ByteArrayContent cloned = new(contentBytes);

            foreach (var header in content.Headers)
            {
                cloned.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return cloned;
        }

        return content;
    }
}
