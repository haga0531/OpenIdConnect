namespace Idp.Models;

public class Context
{
    public List<User> Users { get; set; } = new();

    public List<AuthCode> AuthCodes { get; set; } = new();

    public List<AccessToken> AccessTokens { get; set; } = new();
}
