namespace SigalNET.Gateway.Configuration
{
    public class CorsConfiguration
    {
        public string? Default { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<CorsKeyConfiguration>? Cors { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
