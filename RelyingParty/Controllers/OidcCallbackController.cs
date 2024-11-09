using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RelyingParty.Models;
using RelyingParty.Services;

namespace RelyingParty.Controllers;

public class OidcCallbackController(IHttpClientFactory httpClientFactory, TokenVerifierService tokenVerifierService) : Controller
{
    private const string ClientId = "tiny-client";

    private const string RedirectUri = "http://localhost:4000/oidc/callback";

    private const string TokenEndpoint = "http://localhost:3000/openid-connect/token";

    private const string GrantType = "authorization_code";

    [HttpGet("oidc/callback")]
    public async Task<IActionResult> Index([FromQuery] OidcCallbackRequest request)
    {
        var storedState = HttpContext.Session.GetString("state");
        if (storedState == null || storedState != request.State)
        {
            return StatusCode(400, new { error = "invalid state" });
        }

        var token = await GetTokenAsync(request.Code, request.Scope);

        if (token == null) return View("Error");

        var jwk = await GetJwkAsync();

        var isTokenVerified = tokenVerifierService.VerifyToken(token.IdToken, jwk);

        return View(new OidcCallbackViewModel
        {
            AccessToken = token.AccessToken,
            IdToken = token.IdToken,
            TokenType = token.TokenType,
            ExpiresIn = token.ExpiresIn,
            IsTokenVerified = isTokenVerified
        });
    }

    private async Task<OidcCallbackViewModel?> GetTokenAsync(string code, string scope)
    {
        var client = httpClientFactory.CreateClient();
        var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri),
                new KeyValuePair<string, string>("scope", scope),
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", "c1!3n753cr37")
            }
        );

        var response = await client.PostAsync(TokenEndpoint, content);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            ViewData["ErrorMessage"] = $"Failed to exchange code for token. Status code: {response.StatusCode}, Error: {errorContent}";
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<OidcCallbackViewModel>(responseContent);
    }

    private async Task<string> GetJwkAsync()
    {
        var configuration = await GetConfiguration();
        var jwksUri = configuration["jwks_uri"]?.ToString();
        if (jwksUri == null) throw new Exception("Failed to find JWKS URI.");

        var jwksResponse = await httpClientFactory.CreateClient().GetStringAsync(jwksUri);

        var jwksData = JObject.Parse(jwksResponse);
        var jwk = jwksData["keys"]?.FirstOrDefault(x =>
                x["kty"]?.ToString() == "RSA" &&
                x["alg"]?.ToString() == "RS256" &&
                x["use"]?.ToString() == "sig")
            ?.ToString();
        if (jwk == null) throw new Exception("Failed to find JWK.");

        return jwk;
    }

    private async Task<JObject> GetConfiguration()
    {
        var configurationResponse = await httpClientFactory.CreateClient().GetStringAsync($"http://localhost:3000/openid-connect/.well-known/openid-configuration");
        var configuration = JObject.Parse(configurationResponse);

        return configuration;
    }
}
