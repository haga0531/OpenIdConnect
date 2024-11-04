using Idp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Controllers;

public class LoginController(Context context) : Controller
{
    [HttpPost("/login")]
    public IActionResult Index([FromForm] LoginRequest request)
    {
        if (!Models.User.Login(context.Users, request.Email, request.Password))
        {
            return StatusCode(403, "Unauthorized");
        }

        return Ok(Models.User.FindByEmail(context.Users, request.Email));
    }
}
