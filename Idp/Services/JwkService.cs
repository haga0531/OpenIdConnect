using System.Security.Cryptography;
using Idp.Models;

namespace Idp.Services;

public class JwkService
{
    public Jwk GenerateJwk(string publicKeyPem)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);

        var parameters = rsa.ExportParameters(false);
        if (parameters.Modulus is null || parameters.Exponent is null)
        {
            throw new InvalidOperationException("Invalid RSA key parameters.");
        }

        var n = Base64UrlEncode(parameters.Modulus);
        var e = Base64UrlEncode(parameters.Exponent);

        return new Jwk
        {
            Kty = "RSA",
            Use = "sig",
            Kid = "2024-11-05",
            Alg = "RS256",
            N = n,
            E = e
        };
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}
