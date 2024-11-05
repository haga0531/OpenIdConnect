using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RelyingParty.Services;

public class TokenVerifierService
{
    public bool VerifyToken(string token, string jwk)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(ConvertJwkToPem(jwk));

        var parts = token.Split('.');
        if (parts.Length != 3) throw new ArgumentException("Invalid token format.");

        var encodedHeader = parts[0];
        var encodedPayload = parts[1];
        var encodedSignature = parts[2];

        var decodedSignature = Convert.FromBase64String(Base64UrlDecode(encodedSignature));

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes($"{encodedHeader}.{encodedPayload}");

        return rsa.VerifyData(bytes, decodedSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        string Base64UrlDecode(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2:
                    input += "==";
                    break;
                case 3:
                    input += "=";
                    break;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/')));
        }
    }

    private static string ConvertJwkToPem(string jwk)
    {
        var jwkData = JObject.Parse(jwk);

        var n = jwkData["n"]?.Value<string>();
        var e = jwkData["e"]?.Value<string>();

        if (string.IsNullOrWhiteSpace(n) || string.IsNullOrWhiteSpace(e))
        {
            throw new ArgumentException("Invalid JWK data.");
        }

        var modulus = Base64UrlDecode(n);
        var exponent = Base64UrlDecode(e);

        var rsaParameters = new RSAParameters
        {
            Modulus = modulus,
            Exponent = exponent
        };

        using var rsa = RSA.Create();
        rsa.ImportParameters(rsaParameters);
        var pubicKeyPem = ExportPublicKeyToPem(rsa);

        return pubicKeyPem;

        static byte[] Base64UrlDecode(string value)
        {
            value = value.Replace('-', '+').Replace('_', '/');
            switch (value.Length % 4)
            {
                case 2:
                    value += "==";
                    break;
                case 3:
                    value += "=";
                    break;
            }

            return Convert.FromBase64String(value);
        }

        static string ExportPublicKeyToPem(AsymmetricAlgorithm rsa)
        {
            var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            var sb = new StringBuilder();
            sb.AppendLine("-----BEGIN PUBLIC KEY-----");
            sb.AppendLine(Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks));
            sb.AppendLine("-----END PUBLIC KEY-----");
            return sb.ToString();
        }
    }
}
