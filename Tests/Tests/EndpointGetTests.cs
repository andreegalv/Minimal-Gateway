namespace SigalNET.Gateway.Tests.Tests
{
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SigalNET.Gateway.Configuration;

    [TestClass]
    public class EndpointGetTests
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]

        [TestMethod]
        public async Task CallEndpoint_OnlyURLParameters_Success()
        {
            // Arrange
            var endpointConfiguration = new EndpointConfiguration
            {
                Routes = [
                    new EndpointRoute
                    {
                        UpstreamPathTemplate = "/gateway/security/v1/confirmusername/a.galvez",
                        UpstreamHttpMethod = ["get"],
                        DownstreamPathTemplate = "/api/security/v1/confirmusername/a.galvez",
                        DownstreamServerOptions = new RouteServerOptions
                        {
                            Host = "localhost",
                            Port = 5001
                        }
                    }
                ]
            };

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            var gatewayClientMock = new Mock<GatewayHttpClient>(mockHttpClientFactory.Object);

            gatewayClientMock.Setup(g => g.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("OK")
                });

            using (var app = WebApplicationHelpers.CreateWebApplicationFactory(endpointConfiguration, gatewayClientMock.Object))
            {
                Uri baseUrl = new("http://localhost:5000");

                var httpClient = app.CreateClient();

                // Act
                var response = await httpClient.GetAsync(new Uri("/gateway/security/v1/confirmusername/a.galvez")).ConfigureAwait(false);

                // Asserts
                Assert.IsTrue(response.IsSuccessStatusCode);
            }
        }
    }
}