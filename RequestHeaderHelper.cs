namespace SigalNET.Gateway
{
    using Microsoft.Extensions.Primitives;
    using SigalNET.Gateway.Configuration;

    public static class RequestHeaderHelper
    {
        public static void SetRequiredHeaders(HttpRequestMessage request, EndpointRoute route, IHeaderDictionary headers)
        {
            ArgumentNullException.ThrowIfNull(route);
            ArgumentNullException.ThrowIfNull(request);

            if (route.RequiredHeaders?.Count > 0)
            {
                foreach (string requiredHeader in route.RequiredHeaders)
                {
                    StringValues headerValues = headers.First(h => h.Key == requiredHeader).Value;
                    request.Headers.Add(requiredHeader, headerValues.ToArray());
                }
            }
        }
    }
}
