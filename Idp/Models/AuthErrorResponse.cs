namespace Idp.Models;

public class AuthErrorResponse
{
    public AuthCodeError Error { get; set; }

    public string? ErrorDescription { get; set; }

    public string? ErrorUri { get; set; }
}
