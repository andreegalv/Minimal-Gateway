namespace SigalNET.Gateway.Configuration
{
    public class EndpointConfiguration
    {
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<EndpointRoute>? Routes { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
