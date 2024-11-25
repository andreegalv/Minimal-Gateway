namespace SigalNET.Gateway.Tests.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SigalNET.Gateway.Configuration;

    [TestClass]
    public class UrlHelperTests
    {
        [DataTestMethod]
        [DataRow("/api/v1/sigin/confirmusername/{username}", "http://localhost:5000/api/v1/sigin/confirmusername/a.galvez", "username:a.galvez")]
        [DataRow("/api/v1/product/{id}/detail/{page}", "http://localhost:5000/api/v1/product/1500/detail/3", "id:1500", "page:3")]
        public void TestGeneralApplicationUrls_Success(string downstreamPathTemplate, string expectedPath, params string[] args)
        {
            // Arrange
            EndpointRoute route = new()
            {
                DownstreamPathTemplate = downstreamPathTemplate,
                DownstreamServerOptions = new RouteServerOptions { Host = "localhost", Port = 5000 },

                // Only for test purpose
                UpstreamHttpMethod = [string.Empty],
                UpstreamPathTemplate = string.Empty,
            };

            var routeDictionary = new RouteValueDictionary(args.Select(a =>
            {
                string[] argsSplitted = a.Split(':');
                return new KeyValuePair<string, object?>(argsSplitted[0], argsSplitted[1]);
            }));
            var routeData = new Mock<RouteData>(new RouteData(routeDictionary));

            // Act
            Uri result = UrlHelper.CreateUri(route, routeData.Object);

            // Assert
            Assert.AreEqual(expectedPath, result.ToString());
        }
    }
}
