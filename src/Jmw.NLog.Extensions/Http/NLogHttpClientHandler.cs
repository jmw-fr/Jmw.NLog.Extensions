// <copyright file="NlogHttpClientHandler.cs" company="Jm Weeger">
// Copyright Jm Weeger under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.Extensions.Http
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;

    /// <summary>
    /// <see cref="HttpClientHandler"/> logging requests.
    /// </summary>
    public class NLogHttpClientHandler : HttpClientHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            DateTime dtStart = DateTime.Now;

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Logger.Info($"{request.RequestUri} - {request.Method} - {response.StatusCode} - {(DateTime.Now - dtStart).Milliseconds} ms");

            if (Logger.IsTraceEnabled)
            {
                Logger.Trace(await request.Content.ReadAsStringAsync());
                Logger.Trace(await response.Content.ReadAsStringAsync());
            }

            return response;
        }
    }
}
