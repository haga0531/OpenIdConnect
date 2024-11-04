using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Models;

// https://openid-foundation-japan.github.io/rfc6749.ja.html#code-authz-req
public class AuthRequest : IValidatableObject
{
    [Required]
    [FromQuery(Name = "response_type")]
    public string ResponseType { get; set; } = null!;

    [Required]
    [FromQuery(Name = "client_id")]
    public string ClientId { get; set; } = null!;

    [FromQuery(Name = "redirect_uri")] public string? RedirectUri { get; set; }

    [FromQuery(Name = "scope")] public string? Scope { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validRedirectUris = new List<string> { "http://localhost:4000/oidc/callback" };
        var validClientIds = new List<string> { "tiny-client" };
        var validScopes = new List<string> { "openid" };
        var validResponseTypes = new List<string> { "code" };

        if (string.IsNullOrWhiteSpace(RedirectUri) || !validRedirectUris.Contains(RedirectUri))
        {
            yield return new ValidationResult(AuthCodeError.InvalidRequest.ToString(),
                new[] { ErrorTarget.ResourceOwner.ToString() });
        }

        if (string.IsNullOrWhiteSpace(ClientId) || !validClientIds.Contains(ClientId))
        {
            yield return new ValidationResult(AuthCodeError.InvalidRequest.ToString(),
                new[] { ErrorTarget.ResourceOwner.ToString() });
        }

        if (string.IsNullOrWhiteSpace(ResponseType) || string.IsNullOrWhiteSpace(Scope))
        {
            yield return new ValidationResult(AuthCodeError.InvalidRequest.ToString(),
                new[] { ErrorTarget.RedirectUri.ToString() });
        }

        if (!validResponseTypes.Contains(ResponseType))
        {
            yield return new ValidationResult(AuthCodeError.UnsupportedResponseType.ToString(),
                new[] { ErrorTarget.RedirectUri.ToString() });
        }

        if (Scope != null && !Scope.Split(' ').Any(x => validScopes.Contains(x)))
        {
            yield return new ValidationResult(AuthCodeError.InvalidScope.ToString(),
                new[] { ErrorTarget.RedirectUri.ToString() });
        }
    }
}
