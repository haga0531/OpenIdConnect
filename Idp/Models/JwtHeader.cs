namespace Idp.Models;

public class JwtHeader
{
    public string Alg { get; set; } = null!;

    public string Typ { get; set; } = null!;

    public string Kid { get; set; } = null!;
}
