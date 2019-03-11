// <copyright file="NLogHttpClientHandlerUnitTest.cs" company="Jm Weeger">
// Copyright Jm Weeger under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.NLog.ExtensionsUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using global::NLog;
    using global::NLog.Common;
    using global::NLog.Targets;
    using Jmw.Extensions.Http;
    using Xunit;

    /// <summary>
    /// <see cref="NLogHttpClientHandler"/> unit tests.
    /// </summary>
    public class NLogHttpClientHandlerUnitTest
    {
        /// <summary>
        /// Test logging of an http GET request.
        /// </summary>
        /// <param name="method">Http method.</param>
        /// <param name="uri">Request Uri.</param>
        /// <param name="statusCode">Answer statut code</param>
        /// <param name="requestBody">Request body. Can be <c>null</c>.</param>
        /// <param name="answerBody">Answer body. Can be <c>null</c>.</param>
        [Theory]
        [ClassData(typeof(NLogHttpClientHandlerClassData))]
        public async void LogRequestAsync_MustLog_Request(
            HttpMethod method,
            Uri uri,
            HttpStatusCode statusCode,
            string requestBody,
            string answerBody)
        {
            // Arrange
            var sut = new NLogHttpClientHandler();
            var logs = AddLogger(nameof(NLogHttpClientHandlerUnitTest.LogRequestAsync_MustLog_Request));
            DateTimeOffset dtStart = DateTimeOffset.Now;
            HttpRequestMessage request = new HttpRequestMessage(method, uri);
            HttpResponseMessage response = new HttpResponseMessage(statusCode);

            if (requestBody != null)
            {
                request.Content = new StringContent(requestBody);
            }

            if (answerBody != null)
            {
                response.Content = new StringContent(answerBody);
            }

            // Act
            await sut.LogRequestAsync(dtStart, request, response);

            // Assert
            Assert.NotEmpty(logs);
            LogEventInfo log = logs.First();
            Assert.Equal(request.RequestUri, log.Properties["requestUri"]);
            Assert.Equal(request.Method, log.Properties["requestMethod"]);
            Assert.Equal(response.StatusCode, log.Properties["statusCode"]);
            Assert.Equal(requestBody, log.Properties["requestBody"]);
            Assert.Equal(answerBody, log.Properties["answerBody"]);
        }

        private static IList<LogEventInfo> AddLogger(string name)
        {
            if (LogManager.Configuration == null)
            {
                var config = new global::NLog.Config.LoggingConfiguration();

                InternalLogger.LogLevel = LogLevel.Trace;
                InternalLogger.LogToConsole = true;
                InternalLogger.LogToTrace = true;

                config.AddRuleForAllLevels(new DebugTarget());

                LogManager.Configuration = config;
            }

            List<LogEventInfo> logs = new List<LogEventInfo>();
            var testThread = Thread.CurrentThread;
            MethodCallTarget target = new MethodCallTarget(
                name,
                (logEvent, parms) =>
                {
                    if (testThread == Thread.CurrentThread)
                    {
                        logs.Add(logEvent);
                    }
                });

            LogManager.Configuration.AddRuleForAllLevels(target);

            LogManager.ReconfigExistingLoggers();

            return logs;
        }
    }
}
