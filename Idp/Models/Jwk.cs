namespace Idp.Models;

public class Jwk
{
    public string? Kty { get; set; }

    public string? Use { get; set; }

    public string? Kid { get; set; }

    public string[]? KeyOps { get; set; }

    public string? Alg { get; set; }

    public string? X5U { get; set; }

    public string? X5C { get; set; }

    public string? X5T { get; set; }

    public string? N { get; set; }

    public string? E { get; set; }
}

public class JwkSet
{
    public List<Jwk> Keys { get; set; } = new();
}
