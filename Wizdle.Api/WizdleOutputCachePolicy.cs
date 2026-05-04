namespace Wizdle.Api;

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;

internal sealed class WizdleOutputCachePolicy : IOutputCachePolicy
{
    public static readonly WizdleOutputCachePolicy Instance = new();

    public async ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = true;
        context.AllowCacheStorage = true;
        context.AllowLocking = true;

        context.HttpContext.Request.EnableBuffering();

        using StreamReader reader = new(context.HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
        string body = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        context.HttpContext.Request.Body.Position = 0;

        context.CacheVaryByRules.VaryByValues.Add("body", body);
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        int statusCode = context.HttpContext.Response.StatusCode;
        if (statusCode < 200 || statusCode >= 300)
        {
            context.AllowCacheStorage = false;
        }

        return ValueTask.CompletedTask;
    }
}
