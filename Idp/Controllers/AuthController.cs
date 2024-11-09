using System.ComponentModel.DataAnnotations;
using System.Web;
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
            if (!ModelState.IsValid)
            {
                var validResults = request.Validate(new ValidationContext(request));
                foreach (var result in validResults)
                {
                    var target = result.MemberNames.Contains("RedirectUri")
                        ? ErrorTarget.RedirectUri
                        : ErrorTarget.ResourceOwner;

                    var errorResponse = new AuthErrorResponse
                    {
                        Error = Enum.Parse<AuthCodeError>(result.ErrorMessage),
                        ErrorDescription = result.ErrorMessage,
                    };

                    if (target == ErrorTarget.RedirectUri)
                    {
                        var redirectUrl =
                            $"{request.RedirectUri}?{HttpUtility.UrlEncode(errorResponse.Error.ToString())}={HttpUtility.UrlEncode(errorResponse.ErrorDescription)}";
                        return Redirect(redirectUrl);
                    }

                    return StatusCode(400);
                }
            }

            return View(new AuthViewModel
            {
                ClientId = request.ClientId,
                RedirectUri = request.RedirectUri,
                Scope = request.Scope,
                State = request.State
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
