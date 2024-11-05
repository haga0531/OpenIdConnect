namespace Idp.Models;

public class JwtPayload
{
    public string Sub { get; set; } = null!;

    public string Iss { get; set; } = null!;

    public string Aud { get; set; } = null!;

    public long Exp { get; set; }

    public long Iat { get; set; }
}
