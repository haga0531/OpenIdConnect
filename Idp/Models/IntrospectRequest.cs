using Microsoft.AspNetCore.Mvc;

namespace Idp.Models;

public class IntrospectRequest
{
    [FromForm(Name = "token")]
    public string Token { get; set; } = null!;
}
