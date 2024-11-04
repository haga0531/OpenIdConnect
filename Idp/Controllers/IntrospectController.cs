using Idp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

[Route("openid-connect")]
public class IntrospectController(Context context) : Controller
{
    [HttpPost("introspect")]
    public IActionResult Introspect([FromForm] IntrospectRequest request)
    {
        var foundAccessToken = context.AccessTokens.FirstOrDefault(x => x.Token == request.Token);

        if (foundAccessToken == null || foundAccessToken.ExpiresAt <
            Convert.ToInt32((DateTimeOffset.UtcNow - DateTimeOffset.UnixEpoch).TotalSeconds))
        {
            return Unauthorized(new { active = false });
        }

        return Ok(new { active = true });
    }
}
