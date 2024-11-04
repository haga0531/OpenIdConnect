namespace Idp.Models;

// https://openid.net/specs/openid-connect-core-1_0.html#TokenResponse
public class TokenResponse
{
    public string IdToken { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string TokenType { get; set; } = null!;

    public int ExpiresIn { get; set; }
}
