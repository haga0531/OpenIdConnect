using System.Security.Cryptography;
using System.Text;
using Idp.Models;
using Newtonsoft.Json;

namespace Idp.Services;

public class JwtService
{
    private const int OneDay = 60 * 60 * 24;

    private const string PrivateKeyPath = "./Keys/tiny_idp_private.pem";

    public string Generate(string iss, string aud, string nonce, int expDuration = OneDay)
    {
        var encodedHeader = Base64UrlEncode(JsonConvert.SerializeObject(BuildHeader("2024-11-05")));
        var encodedPayload = Base64UrlEncode(JsonConvert.SerializeObject(BuildPayload(iss, aud, nonce, expDuration)));
        var signature = Sign($"{encodedHeader}.{encodedPayload}");

        return $"{encodedHeader}.{encodedPayload}.{signature}";
    }

    private static string Sign(string target)
    {
        var primaryKey = File.ReadAllText(PrivateKeyPath);
        using var rsa = RSA.Create();
        rsa.ImportFromPem(primaryKey);

        var bytes = Encoding.UTF8.GetBytes(target);
        var signature = rsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return Base64UrlEncode(Convert.ToBase64String(signature));
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

    private static JwtPayload BuildPayload(string iss, string aud, string nonce, int expDuration = OneDay)
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
            Iat = iat,
            Nonce = nonce
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
