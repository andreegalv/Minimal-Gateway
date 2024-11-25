namespace SigalNET.Gateway
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using SigalNET.Gateway.Configuration;
    using SigalNET.Gateway.Filters;

    public static class GatewayEndpoint
    {
        public static void Map(WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var conf = app.Services.GetRequiredService<EndpointConfiguration>();
            if (conf.Routes == null)
            {
                return;
            }

            foreach (var route in conf.Routes)
            {
                MapRoute(app, route);
            }

            CatchAllRoutes(app);
        }

        private static void CatchAllRoutes(WebApplication app)
        {
            app.Map("/{*catchAll}", () =>
            {
                return Results.Problem(statusCode: (int)HttpStatusCode.BadGateway, title: "Bad Gateway");
            })
                .AddEndpointFilter<LoggingEndpointFilter>();
        }

        private static void MapRoute(WebApplication app, EndpointRoute route)
        {
            var routeBuilder = app.MapMethods(route.UpstreamPathTemplate, route.UpstreamHttpMethod, async ([FromServices] GatewayHttpClient gatewayClient, HttpContext context) =>
            {
                HttpMethod method = HttpMethod.Parse(context.Request.Method);

                Uri requestUri = UrlHelper.CreateUri(route, context.GetRouteData());
                using (HttpRequestMessage request = new(method, requestUri))
                {
                    SetHeaders(request, context, route);
                    SetContent(request, context);
                    await CallServicesFromGatewayAsync(gatewayClient, request, context).ConfigureAwait(false);
                }
            });

            SetEndpointFilters(routeBuilder, route);
        }

        private static async Task CallServicesFromGatewayAsync(GatewayHttpClient gatewayClient, HttpRequestMessage request, HttpContext context)
        {
            CancellationToken cancellationToken = context.RequestAborted;

            using (HttpResponseMessage response = await gatewayClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                string detail = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                context.Response.StatusCode = (int)response.StatusCode;
                foreach (var responseHeader in response.Content.Headers)
                {
                    context.Response.Headers.TryAdd(responseHeader.Key, string.Join(";", responseHeader.Value));
                }

                await context.Response.WriteAsync(detail, cancellationToken).ConfigureAwait(false);
            }
        }

        private static void SetContent(HttpRequestMessage request, HttpContext context)
        {
            if (context.Request.Body == null)
            {
                return;
            }

            var streamContent = new StreamContent(context.Request.Body);

            if (context.Request.ContentLength != null)
            {
                streamContent.Headers.ContentLength = context.Request.ContentLength;
            }

            if (!string.IsNullOrEmpty(context.Request.ContentType))
            {
                streamContent.Headers.Add(HeaderNames.ContentType, string.Join(";", context.Request.ContentType));
            }

            request.Content = streamContent;
        }

        private static void SetHeaders(HttpRequestMessage request, HttpContext context, EndpointRoute route)
        {
            RequestHeaderHelper.SetRequiredHeaders(request, route, context.Request.Headers);
            request.Headers.Add("X-Forwarded-For", $"{context.Request.Scheme}://{context.Request.Host.Value}");
        }

        private static void SetEndpointFilters(RouteHandlerBuilder routeBuilder, EndpointRoute route)
        {
            if (!string.IsNullOrEmpty(route.AuthenticationOptions?.Scheme))
            {
                routeBuilder.RequireAuthorization(c =>
                {
                    c.AddAuthenticationSchemes(route.AuthenticationOptions?.Scheme);
                    c.RequireAuthenticatedUser();
                });
            }

            if (!string.IsNullOrEmpty(route.Cors))
            {
                routeBuilder.RequireCors(route.Cors);
            }

            routeBuilder.AddEndpointFilter<LoggingEndpointFilter>();
        }
    }
}
