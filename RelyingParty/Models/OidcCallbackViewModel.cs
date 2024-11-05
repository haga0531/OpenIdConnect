using Newtonsoft.Json;

namespace RelyingParty.Models;

public class OidcCallbackViewModel
{
    [JsonProperty("idToken")]
    public string IdToken { get; set; } = null!;

    [JsonProperty("accessToken")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("tokenType")]
    public string TokenType { get; set; } = null!;

    [JsonProperty("expiresIn")]
    public int ExpiresIn { get; set; }

    public bool IsTokenVerified { get; set; }
}
