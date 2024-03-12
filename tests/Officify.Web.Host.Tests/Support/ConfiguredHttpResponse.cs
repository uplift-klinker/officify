namespace Officify.Web.Host.Tests.Support;

public record ConfiguredHttpResponse(
    HttpMethod Method,
    Uri Url,
    ConfiguredHttpResponseMessage Response,
    Func<byte[], Task<bool>>? Matcher = null,
    int DelayMilliseconds = 0
)
{
    private Func<byte[], Task<bool>> Matcher { get; } = Matcher ?? DefaultMatcher;

    public async Task<HttpResponseMessage?> GetResponse(
        HttpMethod requestMethod,
        Uri requestUrl,
        byte[] content
    )
    {
        await Task.Delay(DelayMilliseconds);
        if (Method != requestMethod)
            return null;

        if (Url.LocalPath != requestUrl.LocalPath)
            return null;

        if (await TryMatchContent(content))
            return Response.ToHttpResponseMessage();

        return null;
    }

    private async Task<bool> TryMatchContent(byte[] content)
    {
        try
        {
            return await Matcher.Invoke(content);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private static Task<bool> DefaultMatcher(byte[] content) => Task.FromResult(true);
}
