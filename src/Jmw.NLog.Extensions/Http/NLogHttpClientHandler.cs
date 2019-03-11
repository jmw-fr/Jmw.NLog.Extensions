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

        /// <summary>
        /// Log the http request using NLog.
        /// </summary>
        /// <param name="dtStart">When the request has been received.</param>
        /// <param name="request">The Http request message.</param>
        /// <param name="response">The http answer.</param>
        /// <returns>Async task.</returns>
        internal async Task LogRequestAsync(DateTimeOffset dtStart, HttpRequestMessage request, HttpResponseMessage response)
        {
            string requestBody = null;
            string requestAnswer = null;

            if (request != null && request.Content != null)
            {
                requestBody = await request.Content.ReadAsStringAsync();
            }

            if (response != null && response.Content != null)
            {
                requestAnswer = await response.Content.ReadAsStringAsync();
            }

            Logger.Info()
                .Property("requestBody", requestBody)
                .Property("answerBody", requestAnswer)
                .Message(
                    "{requestUri} - {requestMethod} - {statusCode} - {processingTime} ms",
                    request.RequestUri,
                    request.Method,
                    response.StatusCode,
                    (DateTime.Now - dtStart).Milliseconds)
                .Write();
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            DateTimeOffset dtStart = DateTimeOffset.Now;

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            await LogRequestAsync(dtStart, request, response);

            return response;
        }
    }
}
