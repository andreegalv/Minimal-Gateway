namespace SigalNET.Gateway.Tests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using SigalNET.Gateway.Authentication;
    using SigalNET.Gateway.Configuration;
    using SigalNET.Gateway.Secret;

    public class TestingWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        public TestingWebApplicationFactory(EndpointConfiguration endpointConfiguration, GatewayHttpClient? gatewayHttpClient = null)
        {
            this._endpointConfiguration = endpointConfiguration;
            this._gatewayHttpClient = gatewayHttpClient;
        }

        private readonly EndpointConfiguration _endpointConfiguration;

        private readonly GatewayHttpClient? _gatewayHttpClient;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ConfigureServices(services =>
            {
                AddAuthorization(services);

                services.RemoveAll<EndpointConfiguration>();
                services.AddSingleton(this._endpointConfiguration);

                if (this._gatewayHttpClient != null)
                {
                    services.RemoveAll<GatewayHttpClient>();
                    services.AddSingleton(this._gatewayHttpClient);
                }
            });

            builder.UseEnvironment("Testing");
        }

        private static void AddAuthorization(IServiceCollection services)
        {
            services.RemoveAll<IJwtAuthenticationFile>();
            services.RemoveAll<ISecretConfigurationFile>();

            services.AddSingleton<IJwtAuthenticationFile>(_ => new JwtAuthenticationFile
            {
                Audience = "gateway.test.audience",
                Issuer = "gateway.test.issuer"
            });
            services.AddSingleton<ISecretConfigurationFile>(_ => new SecretConfigurationFile
            {
                Jwt = "strongPassw0rdFor.,JWTBearerToken09876543*`/",
            });
        }
    }
}
