namespace SigalNET.Gateway
{
    public class GatewayHttpClient : IDisposable
    {
        public GatewayHttpClient(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient();
        }

        ~GatewayHttpClient()
        {
            this.Dispose(false);
        }

        private readonly HttpClient _httpClient;

        private bool _disposed;

        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await this._httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._httpClient.Dispose();
                }

                this._disposed = true;
            }
        }
    }
}
