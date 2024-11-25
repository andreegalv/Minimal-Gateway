namespace SigalNET.Gateway.Authentication
{
    public interface IJwtAuthenticationFile
    {
        string Audience { get; }

        string Issuer { get; }
    }
}
