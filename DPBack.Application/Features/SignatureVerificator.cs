using System.Security.Cryptography;
using System.Text;

namespace DPBack.Application.Features;

public static class SignatureVerificator
{
    public static bool Verify(string rawBody, string signatureHeader, string secondKey)
    {
        var parts = signatureHeader
            .Split(';')
            .Select(p => p.Split('='))
            .ToDictionary(p => p[0], p => p[1]);

        var incomingSignature = parts["signature"];
        var algorithm = parts["algorithm"];

        var data = rawBody + secondKey;

        string computed;

        if (algorithm == "MD5")
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            computed = BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        else
        {
            throw new Exception("Unsupported algorithm");
        }

        return computed == incomingSignature;
    }
}