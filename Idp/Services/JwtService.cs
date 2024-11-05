using System.Text;
using Idp.Models;
using Newtonsoft.Json;

namespace Idp.Services;

public class JwtService
{
    private const int OneDay = 60 * 60 * 24;

    public string Generate(string iss, string aud, int expDuration = OneDay)
    {
        var encodedHeader = Base64UrlEncode(JsonConvert.SerializeObject(BuildHeader("2024-11-05")));
        var encodedPayload = Base64UrlEncode(JsonConvert.SerializeObject(BuildPayload(iss, aud, expDuration)));
        var signTarget = $"{encodedHeader}.{encodedPayload}";
        var signature = Sign(signTarget);

        return $"{signTarget}.{Sign(signature)}";
    }

    private static string Sign(string target)
    {
        return "signature";
    }

    private static JwtHeader BuildHeader(string kid)
    {
        return new JwtHeader
        {
            Alg = "RS256",
            Typ = "JWT",
            Kid = kid
        };
    }

    private static JwtPayload BuildPayload(string iss, string aud, int expDuration = OneDay)
    {
        var sub = Guid.NewGuid().ToString("N");
        var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var exp = iat + expDuration;

        return new JwtPayload
        {
            Iss = iss,
            Sub = sub,
            Aud = aud,
            Exp = exp,
            Iat = iat
        };
    }

    private static string Base64UrlEncode(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);

        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}
