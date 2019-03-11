// <copyright file="NLogHttpClientHandlerClassData.cs" company="Jm Weeger">
// Copyright Jm Weeger under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.NLog.ExtensionsUnitTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    /// <summary>
    /// Test data for <see cref="NLogHttpClientHandlerUnitTest.LogRequestAsync_MustLog_Request"/>
    /// </summary>
    public class NLogHttpClientHandlerClassData : IEnumerable<object[]>
    {
        /// <inheritdoc/>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                HttpMethod.Get,
                new Uri("http://localhost"),
                HttpStatusCode.Accepted,
                null,
                null
            };

            yield return new object[]
            {
                HttpMethod.Get,
                new Uri("http://localhost"),
                HttpStatusCode.OK,
                null,
                "C009E404-75E8-46A2-9760-DC194822CB21"
            };

            yield return new object[]
            {
                HttpMethod.Post,
                new Uri("http://localhost"),
                HttpStatusCode.OK,
                "C009E404-75E9-46A2-9760-DC194822CB21",
                "C009E404-75E8-46A2-9760-DC194822CB21"
            };
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
