using Microsoft.AspNetCore.Mvc;

namespace MyNet.Application.DTOs.Request
{
    public class TokenRequest
    {
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; } = string.Empty;

        [FromForm(Name = "username")]
        public string Username { get; set; } = string.Empty;

        [FromForm(Name = "password")]
        public string Password { get; set; } = string.Empty;

        [FromForm(Name = "scope")]
        public string Scope { get; set; } = string.Empty;

        [FromForm(Name = "client_id")]
        public string ClientId { get; set; } = string.Empty;

        [FromForm(Name = "client_secret")]
        public string ClientSecret { get; set; } = string.Empty;
    }
}
