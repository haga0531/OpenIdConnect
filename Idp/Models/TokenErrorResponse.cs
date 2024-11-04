namespace Idp.Models;

public class TokenErrorResponse
{
    public TokenError Error { get; set; }

    public string? ErrorDescription { get; set; }

    public string? ErrorUri { get; set; }
}
