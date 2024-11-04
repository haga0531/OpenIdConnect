using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RelyingParty.Models;

namespace RelyingParty.Controllers;

public class OidcCallbackController(IHttpClientFactory httpClientFactory) : Controller
{
    private const string ClientId = "tiny-client";

    private const string RedirectUri = "http://localhost:4000/oidc/callback";

    private const string TokenEndpoint = "http://localhost:3000/openid-connect/token";

    private const string GrantType = "authorization_code";

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] OidcCallbackRequest request)
    {
        var token = await GetTokenAsync(request.Code, request.Scope);
        if (token != null)
        {
            return Ok();
        }

        return StatusCode(500, new { error = "Access Token Error" });
    }

    // TODO: Create ViewModel
    private async Task<object?> GetTokenAsync(string code, string scope)
    {
        var client = httpClientFactory.CreateClient();
        var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri),
                new KeyValuePair<string, string>("scope", scope),
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("client_id", ClientId)
            }
        );

        var response = await client.PostAsync(TokenEndpoint, content);
        if (!response.IsSuccessStatusCode) return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject(responseContent);

    }
}
