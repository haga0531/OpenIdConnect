using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Models;

// https://openid-foundation-japan.github.io/rfc6749.ja.html#token-req
public class TokenRequest : IValidatableObject
{
    [Required]
    [FromForm(Name = "grant_type")]
    public string GrantType { get; set; } = null!;

    [Required]
    [FromForm(Name = "client_id")]
    public string ClientId { get; set; } = null!;

    [Required]
    [FromForm(Name = "code")]
    public string Code { get; set; } = null!;

    [Required]
    [FromForm(Name = "redirect_uri")]
    public string RedirectUri { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(GrantType) || string.IsNullOrWhiteSpace(ClientId) || string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(RedirectUri))
        {
            yield return new ValidationResult(TokenError.InvalidRequest.ToString(), new[] { nameof(ClientId), nameof(Code), nameof(RedirectUri) });
        }

        if (GrantType != "authorization_code")
        {
            yield return new ValidationResult(TokenError.UnsupportedGrantType.ToString(), new[] { nameof(GrantType) });
        }
    }
}
