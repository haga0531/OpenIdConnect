using Microsoft.AspNetCore.Mvc;

namespace Idp.Models;

public class TokenRequest
{
    [FromForm(Name = "client_id")]
    public string ClientId { get; set; } = null!;

    [FromForm(Name = "code")]
    public string Code { get; set; } = null!;
}
