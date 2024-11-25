namespace SigalNET.Gateway
{
    using System.Text;
    using SigalNET.Gateway.Configuration;

    public static class UrlHelper
    {
        public static Uri CreateUri(EndpointRoute route, RouteData? routeData = null)
        {
            ArgumentNullException.ThrowIfNull(route);

            StringBuilder downstreamPathBuilder = new(route.DownstreamPathTemplate);

            if (routeData != null)
            {
                foreach (var data in routeData.Values)
                {
                    downstreamPathBuilder.Replace("{" + data.Key + "}", data.Value?.ToString())
                                        .Replace("{*" + data.Key + "}", data.Value?.ToString());
                }
            }

            return new($"http://{route.DownstreamServerOptions.Host}:{route.DownstreamServerOptions.Port}{downstreamPathBuilder}");
        }
    }
}
