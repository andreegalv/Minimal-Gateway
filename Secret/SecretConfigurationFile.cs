namespace SigalNET.Gateway.Secret
{
    public class SecretConfigurationFile : ISecretConfigurationFile
    {
        public required string Jwt { get; set; }
    }
}
