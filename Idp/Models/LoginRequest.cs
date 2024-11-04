using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Idp.Models;

public class LoginRequest
{
    [Required]
    [FromForm(Name = "email")]
    public string Email { get; set; } = null!;

    [Required]
    [FromForm(Name = "password")]
    public string Password { get; set; } = null!;
}
