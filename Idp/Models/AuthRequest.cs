using Microsoft.AspNetCore.Mvc;

namespace Idp.Models;

public class AuthRequest
{
    [FromQuery(Name = "client_id")]
    public string ClientId { get; set; } = null!;

    [FromQuery(Name = "redirect_uri")]
    public string RedirectUri { get; set; } = null!;

    [FromQuery(Name = "scope")]
    public string Scope { get; set; } = null!;
}
