using System.Text;
using Microsoft.AspNetCore.Http;

namespace DPBack.Application.Features;

public static class RawBodyConverter
{
    public static async Task<string> GetRawBody(HttpRequest request)
    {
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();

        request.Body.Position = 0;
        return body;
    }
}