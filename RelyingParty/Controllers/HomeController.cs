using Microsoft.AspNetCore.Mvc;

namespace RelyingParty.Controllers;

public class HomeController : Controller
{
    private const string ClientId = "tiny-client";

    private const string ResponseType = "code";

    private const string Scope = "openid";

    private const string RedirectUri = "http://localhost:4000/oidc/callback";

    private const string AuthorizationEndpoint = "http://localhost:3000/openid-connect/auth";

    public IActionResult Index()
    {
        var state = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("state", state);

        var authorizationUri = $"{AuthorizationEndpoint}" +
                                        $"?client_id={ClientId}" +
                                        $"&response_type={ResponseType}" +
                                        $"&scope={Scope}" +
                                        $"&redirect_uri={RedirectUri}" +
                                        $"&state={state}";

        ViewData["AuthorizationUri"] = authorizationUri;
        return View();
    }
}
