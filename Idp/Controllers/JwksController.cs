using Idp.Models;
using Idp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

[Route("openid-connect")]
public class JwksController(JwkService jwkService) : Controller
{
    private const string PublicKeyPath = "./Keys/tiny_idp_public.pem";

    [HttpGet("jwks")]
    public IActionResult Get()
    {
        var pem = System.IO.File.ReadAllText(PublicKeyPath);

        var jwk = jwkService.GenerateJwk(pem);
        jwk.Kid = "2024-11-05";
        jwk.Alg = "RS256";
        jwk.Use = "sig";

        if (jwk.Kty is null)
        {
            return StatusCode(500, new { error = "Failed to generate jwk." });
        }

        var jwkSet = new JwkSet
        {
            Keys = new List<Jwk> { jwk }
        };

        return Ok(jwkSet);
    }
}
