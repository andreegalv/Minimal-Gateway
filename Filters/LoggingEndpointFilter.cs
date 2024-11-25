namespace SigalNET.Gateway.Filters
{
    using System.Threading.Tasks;

    public partial class LoggingEndpointFilter : IEndpointFilter
    {
        public LoggingEndpointFilter(ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<LoggingEndpointFilter>();
        }

        private readonly ILogger _logger;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            ArgumentNullException.ThrowIfNull(next);

            var url = context?.HttpContext.Request.Path;
            var method = context?.HttpContext.Request.Method;

            LogInformation(this._logger, url, method);

            return await next(context).ConfigureAwait(false);
        }
    }
}
