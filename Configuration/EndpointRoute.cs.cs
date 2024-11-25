namespace SigalNET.Gateway.Configuration
{
    public class EndpointRoute
    {
        public required string UpstreamPathTemplate { get; set; }

        public required string DownstreamPathTemplate { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public required ICollection<string>? UpstreamHttpMethod { get; set; }

        public ICollection<string>? RequiredHeaders { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        public required RouteServerOptions DownstreamServerOptions { get; set; }

        public RouteAuthenticationOptions? AuthenticationOptions { get; set; }

        public string? Cors { get; set; }
    }
}
