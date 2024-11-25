namespace SigalNET.Gateway.Authentication
{
    public class JwtAuthenticationFile : IJwtAuthenticationFile
    {
        public required string Audience { get; set; }

        public required string Issuer { get; set; }
    }
}
