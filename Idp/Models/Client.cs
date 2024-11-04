namespace Idp.Models;

public class Client(string clientId, string clientSecret)
{
    public string ClientId { get; set; } = clientId;

    public string ClientSecret { get; set; } = clientSecret;
}
