using Idp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

public class LoginController(Context context) : Controller
{
    private const string Issuer = "http://localhost:3000";

    [HttpPost("/login")]
    public IActionResult Index(
        [FromForm] string email,
        [FromForm] string password,
        [FromQuery(Name = "client_id")] string clientId,
        [FromQuery(Name = "redirect_uri")] string redirectUri,
        [FromQuery(Name = "scope")] string scope)
    {
        if (!Models.User.Login(context.Users, email, password))
        {
            return StatusCode(403, "Unauthorized");
        }

        var user = Models.User.FindByEmail(context.Users, email);
        var authCode = AuthCode.Build(user.Id, clientId, redirectUri);
        authCode.Save(context.AuthCodes);

        var redirectUrl = $"{redirectUri}?code={authCode.Code}&iss={Issuer}&scope={scope}";
        return Redirect(redirectUrl);
    }
}
