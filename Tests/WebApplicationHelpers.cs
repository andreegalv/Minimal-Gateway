namespace SigalNET.Gateway.Tests
{
    using SigalNET.Gateway.Configuration;

    public static class WebApplicationHelpers
    {
        public static TestingWebApplicationFactory<Program> CreateWebApplicationFactory(EndpointConfiguration endpointConfiguration, GatewayHttpClient? gatewayHttpClient = null)
        {
            return new TestingWebApplicationFactory<Program>(endpointConfiguration, gatewayHttpClient);
        }
    }
}
