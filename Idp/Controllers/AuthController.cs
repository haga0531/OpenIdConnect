using Idp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

[Route("openid-connect")]
public class AuthController : Controller
{
    [HttpGet("auth")]
    public IActionResult GetAuth([FromQuery] AuthRequest request)
    {
        try
        {
            return View(new AuthViewModel
            {
                ClientId = request.ClientId,
                RedirectUri = request.RedirectUri,
                Scope = request.Scope
            });
        }
        catch (Exception e)
        {
            // NOTE: エラー時はserver_errorを返すという仕様も決まっている
            // https://openid-foundation-japan.github.io/rfc6749.ja.html#code-authz-resp
            Console.WriteLine(e);

            return StatusCode(500, new { error = "server_error" });
        }

    }
}
