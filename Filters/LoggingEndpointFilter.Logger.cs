namespace SigalNET.Gateway.Filters
{
    public partial class LoggingEndpointFilter
    {
        [LoggerMessage(Level = LogLevel.Information, Message = "Route \"{Url}\" - \"{Method}\" invoked")]
        private static partial void LogInformation(ILogger logger, string url, string method);
    }
}
