namespace Idp.Models;

public class AuthViewModel
{
    public string ClientId { get; set; } = null!;

    public string RedirectUri { get; set; } = null!;

    public string Scope { get; set; } = null!;

    public string State { get; set; } = null!;
}
