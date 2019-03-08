// <copyright file="NLogHttpClientHandler.cs" company="Jm Weeger">
// Copyright Jm Weeger under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.Extensions.Http
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;
    using NLog.Fluent;

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
            string requestBody = null;
            string requestAnswer = null;

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            requestBody = await request.Content.ReadAsStringAsync();
            requestAnswer = await response.Content.ReadAsStringAsync();

            Logger.Info()
                .Message(
                    "[requestUri} - {requestMethod} - {statusCode} - {processingTime}",
                    request.RequestUri,
                    request.Method,
                    response.StatusCode,
                    (DateTime.Now - dtStart).Milliseconds)
                .Property("requestBody", request.RequestUri)
                .Property("answerBody", request.RequestUri)
                .Write();

            return response;
        }
    }
}
