using System.Text;
using Microsoft.AspNetCore.Http;

namespace DPBack.Application.Features;

public static class RawBodyConverter
{
    public static async Task<string> GetRawBody(HttpRequest request)
    {
        request.EnableBuffering();

        request.Body.Position = 0;

        using var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms);

        var bytes = ms.ToArray();

        request.Body.Position = 0;

        return Encoding.UTF8.GetString(bytes);
    }
}