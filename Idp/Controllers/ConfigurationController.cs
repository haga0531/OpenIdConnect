using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

[Route("openid-connect")]
public class ConfigurationController : Controller
{
    [HttpGet(".well-known/openid-configuration")]
    public IActionResult Get()
    {
        var configuration = new
        {
            issuer = "http://localhost:3000/openid-connect",
            authorization_endpoint = "http://localhost:3000/openid-connect/auth",
            token_endpoint = "http://localhost:3000/openid-connect/token",
            jwks_uri = "http://localhost:3000/openid-connect/jwks",
            response_types_supported = new[] { "code" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "RS256" },
            scopes_supported = new[] { "openid" },
            token_endpoint_auth_methods_supported = new[] { "client_secret_basic" },
            claims_supported = new[] { "sub", "iss" }
        };

        return Ok(configuration);
    }
}
