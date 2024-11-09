using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RelyingParty.Models;

public class OidcCallbackRequest
{
    [Required]
    [FromQuery(Name = "code")]
    public string Code { get; set; } = null!;

    [Required]
    [FromQuery(Name = "scope")]
    public string Scope { get; set; } = null!;

    [Required]
    [FromQuery(Name = "state")]
    public string State { get; set; } = null!;
}
