using Idp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

[Route("openid-connect")]
public class TokenController(Context context) : Controller
{
    [HttpPost("token")]
    public IActionResult PostToken([FromForm] TokenRequest request)
    {
        var authCode = context.AuthCodes.FirstOrDefault(x =>
            x.ClientId == request.ClientId && x.Code == request.Code && x.ExpiredAt > DateTimeOffset.Now);

        authCode!.UsedAt = DateTimeOffset.Now;
        authCode.Save(context.AuthCodes);

        var accessToken = AccessToken.Build(authCode.UserId);
        accessToken.Save(context.AccessTokens);

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
