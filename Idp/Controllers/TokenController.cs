using System.ComponentModel.DataAnnotations;
using Idp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

[Route("openid-connect")]
public class TokenController(Context context) : Controller
{
    [HttpPost("token")]
    public IActionResult PostToken([FromForm] TokenRequest request)
    {
        var validationResults = request.Validate(new ValidationContext(request)).ToList();
        if (validationResults.Count > 0)
        {
            var error = new TokenErrorResponse
            {
                Error = Enum.Parse<TokenError>(validationResults.First().ErrorMessage!)
            };

            return BadRequest(error);
        }

        var authCode = context.AuthCodes.FirstOrDefault(x =>
            x.ClientId == request.ClientId && x.Code == request.Code && x.ExpiredAt > DateTimeOffset.Now);

        // https://openid-foundation-japan.github.io/rfc6749.ja.html#code-authz-resp
        if (authCode is not { UsedAt: null } || authCode.RedirectUri != request.RedirectUri)
        {
            return Unauthorized(new { error = "invalid_grant" });
        }

        authCode!.UsedAt = DateTimeOffset.Now;
        authCode.Save(context.AuthCodes);

        var accessToken = AccessToken.Build(authCode.UserId);
        accessToken.Save(context.AccessTokens);

        var client = context.Clients.FirstOrDefault(x => x.ClientId == request.ClientId);
        if (string.IsNullOrWhiteSpace(client?.ClientSecret) || client.ClientSecret != request.ClientSecret)
        {
            return Unauthorized(new { error = "invalid_client" });
        }

        var response = new TokenResponse
        {
            IdToken = "dummy-id-token",
            AccessToken = accessToken.Token,
            TokenType = "Bearer",
            ExpiresIn = 86400
        };

        Response.Headers.Add("Cache-Control", "no-store");
        Response.Headers.Add("Pragma", "no-cache");

        return Ok(response);
    }
}
